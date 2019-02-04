using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Services.Runtime;
using System.Diagnostics;
using System.Threading;
using Microsoft.Extensions.Logging.EventLog;

namespace AspNet.Core.Template
{
	internal static class Program
	{
		/// <summary>
		/// This is the entry point of the service host process.
		/// </summary>
		private static void Main(string[] args)
		{
			var builder = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json");

			var Configuration = builder.Build();

			try
			{
				if (Configuration.GetValue<string>("AppArchitecture") == "Microservice")
				{
					// The ServiceManifest.XML file defines one or more service type names.
					// Registering a service maps a service type name to a .NET type.
					// When Service Fabric creates an instance of this service type,
					// an instance of the class is created in this host process.

					ServiceRuntime.RegisterServiceAsync("TemisBackendType",
				context => new Core(context)).GetAwaiter().GetResult();

					ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(Core).Name);

					// Prevents this host process from terminating so services keeps running. 
					Thread.Sleep(Timeout.Infinite);
				}
				else
				{
					BuildWebHost(args).Run();
				}
			}
			catch (Exception e)
			{
				if (Configuration.GetValue<string>("AppArchitecture") == "Microservice")
					ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
				throw;
			}
		}

		public static IWebHost BuildWebHost(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseKestrel()
				.UseContentRoot(Directory.GetCurrentDirectory())
				.UseIISIntegration()
				.ConfigureLogging((hostingContext, logging) =>
				{
					logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
					if (hostingContext.Configuration.GetValue("Loggers:Console:Enabled", false))
						logging.AddConsole();
					if (hostingContext.Configuration.GetValue("Loggers:Debug:Enabled", false))
						logging.AddDebug();
					if (hostingContext.Configuration.GetValue("Loggers:EventLog:Enabled", false))
					{
						logging.AddEventLog(new EventLogSettings
						{
							LogName = hostingContext.Configuration.GetValue<string>("Loggers:EventLog:LogName"),
							SourceName = hostingContext.Configuration.GetValue<string>("Loggers:EventLog:SourceName")
						});
					}
					logging.AddEventSourceLogger();
				})
				.UseStartup<Startup>()
				.UseApplicationInsights()
				.Build();
	}
}