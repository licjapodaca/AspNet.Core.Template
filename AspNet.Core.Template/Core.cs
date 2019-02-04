using System.Collections.Generic;
using System.Fabric;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.Extensions.Logging.EventLog;

namespace AspNet.Core.Template
{
	/// <summary>
	/// The FabricRuntime creates an instance of this class for each service type instance. 
	/// </summary>
	internal sealed class Core : StatelessService
	{
		public Core(StatelessServiceContext context)
			: base(context)
		{ }

		/// <summary>
		/// Optional override to create listeners (like tcp, http) for this service instance.
		/// </summary>
		/// <returns>The collection of listeners.</returns>
		protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
		{
			return new ServiceInstanceListener[]
			{
				new ServiceInstanceListener(serviceContext =>
					new KestrelCommunicationListener(serviceContext, "ServiceEndpoint", (url, listener) =>
					{
						ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting Kestrel on {url}");

						return new WebHostBuilder()
									.UseKestrel()
									.ConfigureServices(
										services => services
											.AddSingleton<StatelessServiceContext>(serviceContext))
									.UseContentRoot(Directory.GetCurrentDirectory())
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
									.UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
									.UseUrls(url)
									.Build();
					}))
			};
		}
	}
}
