#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using System.Collections.Generic;
using System.Fabric;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using AspNet.Core.Template.Configurations.Logging;

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
									.ConfigureLoggingMicroservice()
									.UseStartup<Startup>()
									.UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
									.UseUrls(url)
									.Build();
					}))
			};
		}
	}
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member