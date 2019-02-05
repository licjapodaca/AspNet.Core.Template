#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace AspNet.Core.Template.Configurations.Automapper
{
	public static class ConfigureAutomapperServices
	{
		public static void ConfigureAutomapper(this IServiceCollection services)
		{
			services.AddAutoMapper();
		}
	}
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
