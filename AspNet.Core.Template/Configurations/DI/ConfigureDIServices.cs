#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using AspNet.Core.Template.Repositories.Contracts;
using AspNet.Core.Template.Repositories.Implementations;
using AspNet.Core.Template.Services.Contracts;
using AspNet.Core.Template.Services.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNet.Core.Template.Configurations.DI
{
	public static class ConfigureDIServices
	{
		public static void ConfigureDI(this IServiceCollection services, IConfiguration config)
		{
			services.AddHttpContextAccessor();
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddScoped<HttpContext>(p => p.GetService<IHttpContextAccessor>()?.HttpContext);

			services.AddSingleton(config);

			#region Services and Repositories

			// TODO: Especificar las interfaces y clases corrspondientes a los Servicios y Repositorios
			services.AddScoped<ISampleService, SampleService>();
			services.AddScoped<ISampleRepository, SampleRepository>();

			#endregion
		}
	}
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
