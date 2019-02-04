#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.SwaggerUI;

// Tutorial implementing Swagger with Swashbuckle for Asp.Net Core
// Resource: https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-2.1&tabs=visual-studio%2Cvisual-studio-xml
namespace AspNet.Core.Template.Extensions
{
	/// <summary>
	/// Adding security JWT to the swagger calls using the following Extension Methods
	/// Resource: https://ppolyzos.com/2017/10/30/add-jwt-bearer-authorization-to-swagger-and-asp-net-core/
	/// </summary>
	public static class SwaggerServiceExtensions
	{
		public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services, IConfiguration config)
		{
			// Add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
			// note: the specified format code will format the version as "'v'major[.minor][-status]"
			// Issue in this line solved by this resource: https://github.com/Microsoft/aspnet-api-versioning/issues/330
			services.AddApiVersioning();

			//services.AddMvcCore().AddVersionedApiExplorer(
			services.AddVersionedApiExplorer(
				options =>
				{
					options.GroupNameFormat = "'v'VVV";

					// note: this option is only necessary when versioning by url segment. the SubstitutionFormat
					// can also be used to control the format of the API version in route templates
					options.SubstituteApiVersionInUrl = true;
				});

			// API Versioning resources:
			// Blog: https://neelbhatt.com/2018/04/21/api-versioning-in-net-core/
			// WiKi: https://github.com/Microsoft/aspnet-api-versioning/wiki
			services.AddApiVersioning(o =>
			{
				o.ReportApiVersions = true;
				o.AssumeDefaultVersionWhenUnspecified = true;
				o.DefaultApiVersion = new ApiVersion(
					config.GetValue<int>("App-Parameters:Swagger:DefaultApiVersion:majorVersion"),
					config.GetValue<int>("App-Parameters:Swagger:DefaultApiVersion:minorVersion"));
			});

			services.AddSwaggerGen(
				options =>
				{
					// resolve the IApiVersionDescriptionProvider service
					// note: that we have to build a temporary service provider here because one has not been created yet
					var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

					// add a swagger document for each discovered API version
					// note: you might choose to skip or document deprecated API versions differently
					foreach (var description in provider.ApiVersionDescriptions)
					{
						options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description, config));
					}

					// add a custom operation filter which sets default values
					options.OperationFilter<SwaggerDefaultValues>();

					// integrate xml comments
					options.IncludeXmlComments(XmlCommentsFilePath);

					var security = new Dictionary<string, IEnumerable<string>>
						{
							{ "Bearer", new string[] { } }
						};

					options.AddSecurityDefinition("Bearer", new ApiKeyScheme
					{
						Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
						Name = "Authorization",
						In = "header",
						Type = "apiKey"
					});

					options.AddSecurityRequirement(security);
				});

			return services;
		}

		public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app, IApiVersionDescriptionProvider provider, IConfiguration config)
		{
			app.UseSwagger();

			app.UseSwaggerUI(
				options =>
				{
					options.RoutePrefix = config.GetValue<string>("App-Parameters:Swagger:UrlHome");

					// build a swagger endpoint for each discovered API version
					foreach (var description in provider.ApiVersionDescriptions)
					{
						options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
					}

					options.DocExpansion(DocExpansion.None);
				});

			return app;
		}

		static string XmlCommentsFilePath
		{
			get
			{
				var basePath = PlatformServices.Default.Application.ApplicationBasePath;
				var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
				return Path.Combine(basePath, fileName);
			}
		}

		static Info CreateInfoForApiVersion(ApiVersionDescription description, IConfiguration config)
		{
			var info = new Info()
			{
				Title = $"{config.GetValue<string>("App-Parameters:Swagger:Informacion:Titulo")} {description.ApiVersion}",
				Version = description.ApiVersion.ToString(),
				Description = config.GetValue<string>("App-Parameters:Swagger:Informacion:Descripcion"),
				Contact = new Contact()
				{
					Name = config.GetValue<string>("App-Parameters:Swagger:Informacion:Contacto:Nombre"),
					Email = config.GetValue<string>("App-Parameters:Swagger:Informacion:Contacto:Email"),
					Url = config.GetValue<string>("App-Parameters:Swagger:Informacion:Contacto:Url")
				},
				TermsOfService = config.GetValue<string>("App-Parameters:Swagger:Informacion:TerminosDeServicio"),
				License = new License()
				{
					Name = config.GetValue<string>("App-Parameters:Swagger:Informacion:Licencia:Nombre"),
					Url = config.GetValue<string>("App-Parameters:Swagger:Informacion:Licencia:Url")
				}
			};

			if (description.IsDeprecated)
			{
				info.Description += config.GetValue<string>("App-Parameters:Swagger:Informacion:DescripcionObsoleto");
			}

			return info;
		}
	}

	public class SwaggerDefaultValues : IOperationFilter
	{
		public void Apply(Operation operation, OperationFilterContext context)
		{
			if (operation.Parameters == null)
			{
				return;
			}

			foreach (var parameter in operation.Parameters.OfType<NonBodyParameter>())
			{
				var description = context.ApiDescription
										 .ParameterDescriptions
										 .First(p => p.Name == parameter.Name);
				var routeInfo = description.RouteInfo;

				if (parameter.Description == null)
				{
					parameter.Description = description.ModelMetadata?.Description;
				}

				if (routeInfo == null)
				{
					continue;
				}

				if (parameter.Default == null)
				{
					parameter.Default = routeInfo.DefaultValue;
				}

				parameter.Required |= !routeInfo.IsOptional;
			}
		}
	}
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
