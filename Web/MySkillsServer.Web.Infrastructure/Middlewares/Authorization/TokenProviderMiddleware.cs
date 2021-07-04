namespace MySkillsServer.Web.Infrastructure.Middlewares.Authorization
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Net;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;

    using MySkillsServer.Common;
    using MySkillsServer.Web.Common;

    //// JWT Authentication services 2
    public class TokenProviderMiddleware
    {
        private readonly RequestDelegate next;
        private readonly TokenProviderOptions options;
        private readonly Func<HttpContext, Task<GenericPrincipal>> principalResolver;

        public TokenProviderMiddleware(
            RequestDelegate next,
            IOptions<TokenProviderOptions> options,
            Func<HttpContext, Task<GenericPrincipal>> principalResolver)
        {
            this.next = next;
            this.options = options.Value;
            this.principalResolver = principalResolver;
        }

        public Task Invoke(HttpContext context)
        {
            if (!context.Request.Path.Equals(this.options.Path, StringComparison.Ordinal))
            {
                return this.next(context);
            }

            // if (context.Request.Method.Equals("POST"))
            if (context.Request.Method.Equals("POST") && context.Request.HasFormContentType)
            {
                return this.GenerateToken(context);
            }

            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return context.Response.WriteAsync("Bad request");
        }

        private static int GetClaimIndex(IList<Claim> claims, string type)
        {
            for (var i = 0; i < claims.Count; i++)
            {
                if (claims[i].Type == type)
                {
                    return i;
                }
            }

            return -1;
        }

        private async Task GenerateToken(HttpContext context)
        {
            var principal = await this.principalResolver(context);
            if (principal == null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                // this is in the body as text, not a json object
                // await context.Response.WriteAsync("Invalid email or password.");
                context.Response.ContentType = GlobalConstants.JsonContentType;
                var error = new ErrorResponseModel { Description = "Invalid email or password." };
                await context.Response.WriteAsync(JsonSerializer.Serialize(error));

                return;
            }

            var now = DateTime.UtcNow;
            var unixTimeSeconds = (long)Math.Round(
                (now.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

            var existingClaims = principal.Claims.ToList();

            var systemClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, principal.Identity.Name),
                new Claim(JwtRegisteredClaimNames.Jti, await this.options.NonceGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, unixTimeSeconds.ToString(), ClaimValueTypes.Integer64),
            };

            foreach (var systemClaim in systemClaims)
            {
                var existingClaimIndex = GetClaimIndex(existingClaims, systemClaim.Type);
                if (existingClaimIndex < 0)
                {
                    existingClaims.Add(systemClaim);
                }
                else
                {
                    existingClaims[existingClaimIndex] = systemClaim;
                }
            }

            var jwt = new JwtSecurityToken(
                issuer: this.options.Issuer,
                audience: this.options.Audience,
                claims: existingClaims,
                notBefore: now,
                expires: now.Add(this.options.Expiration),
                signingCredentials: this.options.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new TokenProviderUserResponseModel
            {
                AccessToken = encodedJwt,
                ExpiresIn = (int)this.options.Expiration.TotalMilliseconds,
                Roles = existingClaims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value),
                UserEmail = existingClaims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value,
            };

            context.Response.ContentType = GlobalConstants.JsonContentType;
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
