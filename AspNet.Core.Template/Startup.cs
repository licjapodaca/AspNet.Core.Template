#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#region Usings
using AspNet.Core.Template.Configurations.Cors;
using AspNet.Core.Template.Configurations.Database;
using AspNet.Core.Template.Configurations.DI;
using AspNet.Core.Template.Configurations.Mvc;
using AspNet.Core.Template.Configurations.Swagger;
using AspNet.Core.Template.CustomMiddleware;
using AspNet.Core.Template.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
#endregion

namespace AspNet.Core.Template
{
	public class Startup
	{
		private readonly ILogger _logger;
		private readonly IConfiguration _config;

		public Startup(IHostingEnvironment hostingEnvironment, ILogger<Startup> logger)
		{
			_config = new ConfigurationBuilder()
				.SetBasePath(hostingEnvironment.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddEnvironmentVariables()
				.Build();
			_logger = logger;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.ConfigureCors();
			services.ConfigureDI(_config);
			services.ConfigureDatabase(_config);
			services.ConfigureSwagger(_config);
			services.ConfigureMvc();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseGlobalExceptionMiddleware();
			app.UseAuthorizationTokenReceptionMiddleware();
			app.UseCors("CorsPolicy");
			app.UseSwaggerDocumentation(provider, _config);
			app.UseStaticFiles();
			app.UseMvc();
		}
	}
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
