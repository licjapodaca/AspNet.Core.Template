#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using AspNet.Core.Template.CustomMiddleware.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace AspNet.Core.Template.CustomMiddleware
{
	public static class CustomMiddlewareExtensions
	{
		public static IApplicationBuilder UseAuthorizationTokenReceptionMiddleware(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<AuthorizationTokenReceptionMiddleware>();
		}

		public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
		}
	}
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
