namespace MySkillsServer.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using MySkillsServer.Common;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Web.Infrastructure.Middlewares.Authorization;
    using MySkillsServer.Web.Infrastructure.Settings;
    using MySkillsServer.Web.ViewModels.Accounts;

    public class AccountsService : IAccountsService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IOptions<TokenProviderOptions> options;
        private readonly IOptions<JwtSettings> jwtSettings;

        public AccountsService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IOptions<TokenProviderOptions> options,
            IOptions<JwtSettings> jwtSettings)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.options = options;
            this.jwtSettings = jwtSettings;
        }

        public async Task<UserLoginResponseModel> Authenticate(ApplicationUser user)
        {
            // Get valid claims and pass them into JWT
            var now = DateTime.UtcNow;

            var claims = await this.GetValidClaims(user);
            var jwtSecretKey = Encoding.ASCII.GetBytes(this.jwtSettings.Value.Secret);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = this.options.Value.Audience,
                Issuer = this.options.Value.Issuer,
                NotBefore = now,
                Subject = new ClaimsIdentity(claims),
                Expires = now.Add(this.options.Value.Expiration),
                SigningCredentials = this.options.Value.SigningCredentials,
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenAsString = tokenHandler.WriteToken(token);

            return new UserLoginResponseModel
            {
                accessToken = tokenAsString,
                expiresIn = (int)this.options.Value.Expiration.TotalMilliseconds,
                roles = claims.Where(c => c.Type == ClaimTypes.Role).Select(r => r.Value),
                userEmail = claims.Find(c => c.Type == ClaimTypes.Email).Value,
            };
        }

        private async Task<List<Claim>> GetValidClaims(ApplicationUser user)
        {
            var now = DateTime.UtcNow;
            var unixTimeSeconds = (long)Math.Round(
                (now.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, unixTimeSeconds.ToString(), ClaimValueTypes.Integer64),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Email),
            };

            var userClaims = await this.userManager.GetClaimsAsync(user);
            var userRoles = await this.userManager.GetRolesAsync(user);

            claims.AddRange(userClaims);

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await this.roleManager.FindByNameAsync(userRole);
                if (role != null)
                {
                    var roleClaims = await this.roleManager.GetClaimsAsync(role);
                    foreach (Claim roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
            }

            return claims;
        }

        ////old method - problem with Roles
        //public async Task<UserLoginResponseModel> Authenticate(ApplicationUser user)
        //{
        //    var claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Email, user.Email),
        //        new Claim(ClaimTypes.NameIdentifier, user.Id),
        //    };

        //    //// Add roles ids as multiple claims
        //    // var roles = await this.userManager.GetRolesAsync(user);
        //    // foreach (var role in roles)
        //    // {
        //    //    claims.Add(new Claim(ClaimTypes.Role, role));
        //    // }
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var jwtSecretKey = Encoding.ASCII.GetBytes(this.jwtSettings.Value.Secret);
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Audience = this.jwtSettings.Value.Audience,
        //        Issuer = this.jwtSettings.Value.Issuer,

        //        Subject = new ClaimsIdentity(new Claim[]
        //                            {
        //                                new Claim(ClaimTypes.Email, user.Email),
        //                                new Claim(ClaimTypes.NameIdentifier, user.Id),

        //                                // new Claim(ClaimTypes.Role, GlobalConstants.AdministratorRoleName),
        //                            }),

        //        // Subject = new ClaimsIdentity(claims),
        //        Expires = DateTime.UtcNow.AddDays(7),
        //        SigningCredentials = new SigningCredentials(
        //            new SymmetricSecurityKey(jwtSecretKey),
        //            SecurityAlgorithms.HmacSha256Signature),
        //    };

        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    var tokenAsString = tokenHandler.WriteToken(token);

        //    return new UserLoginResponseModel
        //    {
        //        AccessToken = tokenAsString,
        //    };
        //}
    }
}
