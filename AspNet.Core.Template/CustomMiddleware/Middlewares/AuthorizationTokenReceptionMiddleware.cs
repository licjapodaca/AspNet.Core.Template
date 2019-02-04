#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace AspNet.Core.Template.CustomMiddleware.Middlewares
{
	public class AuthorizationTokenReceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger _logger;

		public AuthorizationTokenReceptionMiddleware(RequestDelegate next, ILogger<AuthorizationTokenReceptionMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			// Logic to perform on request
			_logger.LogInformation(5001, "Iniciando peticion y pasando por AuthorizationTokenReceptionMiddleware...");

			var claimsPrincipal = SetUserPrincipal(context);

			if (claimsPrincipal != null)
			{
				Thread.CurrentPrincipal = claimsPrincipal;
				context.User = claimsPrincipal;
				_logger.LogInformation(5001, $"Autorizacion: {context.User.Identity.Name}");
			}

			await _next(context);

			// Logic to perform on response
			_logger.LogInformation(5001, "Terminando peticion y pasando nuevamente por AuthorizationTokenReceptionMiddleware...");

		}

		#region Private Methods

		private ClaimsPrincipal SetUserPrincipal(HttpContext context)
		{
			var Claims = new List<Claim>();

			try
			{
				if (!string.IsNullOrEmpty(context.Request.Headers["Authorization"].ToString()))
				{
					var token = context.Request.Headers["Authorization"].ToString().StartsWith("Bearer ") ?
						context.Request.Headers["Authorization"].ToString().Substring(7) :
						context.Request.Headers["Authorization"].ToString();

					JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
					JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);

					TokenValidationParameters parameters = new TokenValidationParameters()
					{
						ValidateIssuerSigningKey = true,
						ValidateIssuer = true,
						ValidateAudience = false,
						ValidIssuer = jwtToken.Issuer,
						IssuerSigningKey = new X509SecurityKey(GetCertificate("BTSOneIdentityServer"))
					};

					var claimsPrincipal = tokenHandler.ValidateToken(token, parameters, out SecurityToken securityToken);

					return claimsPrincipal;
				}

				return null;
			}
			catch (Exception)
			{
				throw;
			}
		}

		private X509Certificate2 GetCertificate(string issuerName)
		{
			try
			{
				var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);

				store.Open(OpenFlags.ReadOnly);

				var certificates = store.Certificates.Find(X509FindType.FindByIssuerName, issuerName, false);

				if (certificates.Count > 0)
				{
					return certificates[0];
				}
				else
					return null;
			}
			catch (Exception)
			{
				throw;
			}
		}

		#endregion
	}
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
