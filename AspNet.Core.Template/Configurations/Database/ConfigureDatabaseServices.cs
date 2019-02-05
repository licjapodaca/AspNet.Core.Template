#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using AspNet.Core.Template.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;

namespace AspNet.Core.Template.Configurations.Database
{
	public static class ConfigureDatabaseServices
	{
		private static IConfiguration _config;

		public static void ConfigureDatabase(this IServiceCollection services, IConfiguration config)
		{
			_config = config;

			#region DI for EntityFramework
			services.AddDbContext<MainContext>();
			var descriptor = new ServiceDescriptor(typeof(DbContextOptions<MainContext>), DbContextOptionsFactory<MainContext>, ServiceLifetime.Scoped);
			services.Replace(descriptor);
			#endregion
		}

		#region Private Methods

		private static DbContextOptions<T> DbContextOptionsFactory<T>(IServiceProvider provider) where T : DbContext
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

		private static string ObtenerConnectionString(IHttpContextAccessor httpAccesor)
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
