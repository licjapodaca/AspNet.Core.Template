#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using Microsoft.Extensions.DependencyInjection;

namespace AspNet.Core.Template.Configurations.Cors
{
	public static class ConfigureCorsServices
	{
		public static void ConfigureCors(this IServiceCollection services)
		{
			services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
			{
				builder
					.AllowAnyMethod()
					.AllowAnyHeader()
					.AllowAnyOrigin();
			}));
		}
	}
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
