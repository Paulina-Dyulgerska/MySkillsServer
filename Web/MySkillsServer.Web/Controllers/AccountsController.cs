namespace MySkillsServer.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MySkillsServer.Common;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Data;
    using MySkillsServer.Web.ViewModels.Accounts;

    [ApiController]
    [Route("/api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsService accountsService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountsController(
            IAccountsService accountsService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this.accountsService = accountsService;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> WhoAmI()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            return this.Ok(user.UserName);
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestModel input)
        {
            var user = await this.userManager.FindByEmailAsync(input.Email);

            if (user == null || user.IsDeleted)
            {
                return this.BadRequest(new { Message = "No such user" });
            }

            var validCredentials = await this.userManager.CheckPasswordAsync(user, input.Password);

            if (!validCredentials)
            {
                return this.BadRequest(new { Message = "Email or password is incorrect" });
            }

            // sample code to run if user's credentials is valid and before login
            //if (!await this.userManager.IsInRoleAsync(user, GlobalConstants.AdministratorRoleName))
            //{
            //    return this.BadRequest(new { Message = "You need higher permission to access this functionality" });
            //}

            var token = this.accountsService.Authenticate(user);

            var result = await this.signInManager
                .PasswordSignInAsync(input.Email, input.Password, isPersistent: false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return this.BadRequest(new { Message = "Invalid login attempt" });
            }

            return this.Ok(token);
        }

        private async Task<GenericPrincipal> PrincipalResolver(UserLoginRequestModel input)
        {
            var user = await this.userManager.FindByEmailAsync(input.Email);
            if (user == null || user.IsDeleted)
            {
                return null;
            }

            var isValidPassword = await this.userManager.CheckPasswordAsync(user, input.Password);
            if (!isValidPassword)
            {
                return null;
            }

            var roles = await this.userManager.GetRolesAsync(user);

            var identity = new GenericIdentity(user.Email, "Token");
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));

            return new GenericPrincipal(identity, roles.ToArray());
        }
    }
}
