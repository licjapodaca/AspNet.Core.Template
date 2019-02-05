#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace AspNet.Core.Template.Configurations.Mvc
{
	public static class ConfigureMvcServices
	{
		public static void ConfigureMvc(this IServiceCollection services)
		{
			#region MVC Configuration
			services
				.AddMvc()
				.AddJsonOptions(options =>
				{
					options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
					options.SerializerSettings.ContractResolver = new DefaultContractResolver();
				})
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
			#endregion
		}
	}
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
