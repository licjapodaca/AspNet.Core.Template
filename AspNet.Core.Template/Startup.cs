#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using AspNet.Core.Template.CustomMiddleware;
using AspNet.Core.Template.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;

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
			_logger.LogInformation(5000, "Entrando a la DI de ASP.NET Core...");

			#region CORS
			services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
			{
				builder
					.AllowAnyMethod()
					.AllowAnyHeader()
					.AllowAnyOrigin();
			}));
			#endregion

			#region DI to get Claims from header Token Request
			services.AddHttpContextAccessor();
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddScoped<HttpContext>(p => p.GetService<IHttpContextAccessor>()?.HttpContext);
			#endregion

			#region DI for Configuration Files
			services.AddSingleton(_config);
			#endregion

			#region DI for EntityFramework
			//TODO: Especificar el nombre correcto del DbContext perteneciente al Microservicio
			services.AddDbContext<DbContext>();
			var descriptor = new ServiceDescriptor(typeof(DbContextOptions<DbContext>), DbContextOptionsFactory<DbContext>, ServiceLifetime.Scoped);
			services.Replace(descriptor);
			#endregion

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

			#region Swagger Documentation
			services.AddSwaggerDocumentation(_config);
			#endregion
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

		#region Private Methods

		private DbContextOptions<T> DbContextOptionsFactory<T>(IServiceProvider provider) where T : DbContext
		{
			var httpContext = provider.GetService<IHttpContextAccessor>();
			var connectionString = string.Empty;

			connectionString = ObtenerConnectionString(httpContext);

			var optionBuilder = new DbContextOptionsBuilder<T>();
			optionBuilder
				.UseSqlServer(
					connectionString,
					options =>
					{
						options.MaxBatchSize(150);
						options.EnableRetryOnFailure(3, TimeSpan.FromSeconds(1), null);
					});

			return optionBuilder.Options;
		}

		private string ObtenerConnectionString(IHttpContextAccessor httpAccesor)
		{
			var connectionString = string.Empty;

			if (httpAccesor.HttpContext == null)
			{
				connectionString = _config.GetConnectionString("DefaultConnection");
			}
			else
			{
				var qConnStr = httpAccesor.HttpContext.User.Claims.FirstOrDefault(p => p.Type == "CONNECTIONSTRING");
				if (qConnStr != null)
				{
					// TODO: Descomentar y agregar referencia al proyecto BTS.BtsOne.Tools.Criptografia
					//connectionString = CrypthoService.DesencriptarRijndaelTokenService(qConnStr.Value);
				}
				else
				{
					throw new NullReferenceException("No se encontro el valor de conexion a la base de datos");
				}
			}

			return connectionString;
		}

		#endregion
	}
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
