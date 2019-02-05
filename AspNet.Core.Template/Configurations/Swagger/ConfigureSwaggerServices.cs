#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using AspNet.Core.Template.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNet.Core.Template.Configurations.Swagger
{
	public static class ConfigureSwaggerServices
	{
		public static void ConfigureSwagger(this IServiceCollection services, IConfiguration config)
		{
			#region Swagger Documentation
			services.AddSwaggerDocumentation(config);
			#endregion
		}
	}
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
