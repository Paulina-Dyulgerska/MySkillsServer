namespace MySkillsServer.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using MySkillsServer.Common;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Web.Infrastructure.Settings;
    using MySkillsServer.Web.ViewModels.Accounts;

    public class AccountsService : IAccountsService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IOptions<JwtSettings> jwtSettings;

        public AccountsService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IOptions<JwtSettings> jwtSettings)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.jwtSettings = jwtSettings;
        }

        public async Task<UserLoginResponseModel> Authenticate(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            // Add roles ids as multiple claims
            var roles = await this.userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecretKey = Encoding.ASCII.GetBytes(this.jwtSettings.Value.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = this.jwtSettings.Value.Audience,
                Issuer = this.jwtSettings.Value.Issuer,

                //Subject = new ClaimsIdentity(new Claim[]
                //                    {
                //                        new Claim(ClaimTypes.Email, user.Email),
                //                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                //                        new Claim(ClaimTypes.Role, GlobalConstants.AdministratorRoleName),
                //                    }),
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(jwtSecretKey),
                    SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenAsString = tokenHandler.WriteToken(token);

            return new UserLoginResponseModel
            {
                AccessToken = tokenAsString,
            };
        }
    }
}
