#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.ServiceFabric.Services.Runtime;
using System.Diagnostics;
using System.Threading;
using AspNet.Core.Template.Configurations.Logging;

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

					ServiceRuntime.RegisterServiceAsync("TemisBackendType", context => new Core(context)).GetAwaiter().GetResult();

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
				.ConfigureLoggingMicroservice()
				.UseStartup<Startup>()
				.UseApplicationInsights()
				.Build();
	}
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member