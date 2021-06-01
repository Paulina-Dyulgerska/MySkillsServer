﻿namespace MySkillsServer.Web.Infrastructure.Middlewares.Authorization
{
    using System;
    using System.Security.Principal;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;

    //// JWT Authentication services 2
    public static class TokenProviderExtensions
    {
        public static void UseJwtBearerTokens(
            this IApplicationBuilder app,
            IOptions<TokenProviderOptions> options,
            Func<HttpContext, Task<GenericPrincipal>> principalResolver)
        {
            ValidateArgs(app, options, principalResolver);

            app.UseMiddleware<TokenProviderMiddleware>(options, principalResolver);

            app.UseAuthentication();
        }

        private static void ValidateArgs(
            IApplicationBuilder app,
            IOptions<TokenProviderOptions> options,
            Func<HttpContext, Task<GenericPrincipal>> principalResolver)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (options?.Value == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (principalResolver == null)
            {
                throw new ArgumentNullException(nameof(principalResolver));
            }
        }
    }
}
