namespace MySkillsServer.Web.Controllers
{
    using System;
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
    using MySkillsServer.Services.Mapping;
    using MySkillsServer.Web.ViewModels.Accounts;

    [ApiController]
    [Route("/api/[controller]")]
    public class AccountsController : BaseController
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
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> WhoAmI()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var userEmail = this.User.FindFirst(ClaimTypes.Name).Value;

            return this.Ok(userEmail);
        }

        ////// JWT Authentication services 1
        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] UserLoginRequestModel input)
        //{
        //    var user = await this.userManager.FindByEmailAsync(input.Email);

        //    if (user == null || user.IsDeleted)
        //    {
        //        return this.BadRequest(new { Message = "No such user" });
        //    }

        //    var validCredentials = await this.userManager.CheckPasswordAsync(user, input.Password);

        //    if (!validCredentials)
        //    {
        //        return this.BadRequest(new { Message = "Email or password is incorrect" });
        //    }

        //    // sample code to run if user's credentials is valid and before login
        //    // if (!await this.userManager.IsInRoleAsync(user, GlobalConstants.AdministratorRoleName))
        //    // {
        //    //    return this.BadRequest(new { Message = "You need higher permission to access this functionality" });
        //    // }

        //    var result = await this.signInManager
        //        .PasswordSignInAsync(input.Email, input.Password, isPersistent: false, lockoutOnFailure: false);

        //    if (!result.Succeeded)
        //    {
        //        return this.BadRequest(new { Message = "Invalid login attempt" });
        //    }

        //    var token = await this.accountsService.Authenticate(user);

        //    // return this.Ok(this.User.Identity.IsAuthenticated);
        //    return this.Ok(token.AccessToken);
        //}

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequestModel input)
        {

            if (input == null || !this.ModelState.IsValid)
            {
                return this.BadRequest(new { Message = "Invalid register attempt" });
            }

            if (input.Password == input.ConfirmPassword
                || string.IsNullOrWhiteSpace(input.Password)
                || string.IsNullOrWhiteSpace(input.ConfirmPassword))
            {
                return this.BadRequest(new { Message = "Passwords must match and should not be empty" });
            }

            var user = input.To<ApplicationUser>();
            //user.BlogList.Add(new BlogList { IsSystem = true, Name = PlaylistsConstants.CurrentPlaylistName });
            //user.Playlists.Add(new Playlist { IsSystem = true, Name = PlaylistsConstants.LikesPlaylistName });

            var result = await this.userManager.CreateAsync(user, input.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }

                return this.BadRequest(this.ModelState);
            }

            await this.userManager.AddToRoleAsync(user, GlobalConstants.GuestRoleName);

            await this.signInManager.PasswordSignInAsync(input.Email, input.Password, isPersistent: false, lockoutOnFailure: false);

            return this.Ok(new UserRegisterResponseModel
            {
                Id = user.Id,
            });
        }

        public Task<IActionResult> Logout([FromBody] UserRegisterRequestModel input)
        {
            // TODO this in the Client!!!!!!!!!!
            // @page "/account/logout"

            // < h1 > Logging out...</ h1 >

            // @code {
            //    protected override async Task OnInitializedAsync()
            //        {
            //            await this.JsRuntime.DeleteToken();
            //            this.State.Username = null;
            //            this.State.UserToken = null;
            //            this.StateHasChanged();
            //            this.NavigationManager.NavigateTo("/");
            //        }
            //    }
            return null;
        }
    }
}
