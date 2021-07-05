namespace MySkillsServer.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MySkillsServer.Common;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Data;
    using MySkillsServer.Services.Mapping;
    using MySkillsServer.Web.Common;
    using MySkillsServer.Web.ViewModels.Accounts;

    [ApiController]
    [Route("/api/[controller]")]
    public class AccountsController : BaseController
    {
        private readonly IAccountsService accountsService;
        private readonly IReCaptchaService googleReCaptchaService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountsController(
            IAccountsService accountsService,
            IReCaptchaService googleReCaptchaService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this.accountsService = accountsService;
            this.googleReCaptchaService = googleReCaptchaService;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.UserRoleName)]
        public async Task<IActionResult> WhoAmI()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var userEmail = this.User.FindFirst(ClaimTypes.Name).Value;
            var userRoles = this.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(r => r.Value);

            return this.Ok(new
            {
                UserEmail = userEmail,
                UserRoles = userRoles,
            });

            //var userWIthTokenAndClaims = this.accountsService.Authenticate(user);

            //return this.Ok(userWIthTokenAndClaims);
        }

        //// JWT Authentication services 1
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] UserLoginRequestModel input)
        {
            var user = await this.userManager.FindByEmailAsync(input.Email);

            if (user == null || user.IsDeleted)
            {
                return this.BadRequest(new ErrorResponseModel
                {
                    Description = "No such user",
                });
            }

            var validCredentials = await this.userManager.CheckPasswordAsync(user, input.Password);

            if (!validCredentials)
            {
                return this.BadRequest(new ErrorResponseModel
                {
                    Description = "Email or password is incorrect",
                });
            }

            // sample code to run if user's credentials is valid and before login
            // if (!await this.userManager.IsInRoleAsync(user, GlobalConstants.AdministratorRoleName))
            // {
            //    return this.BadRequest(new { Message = "You need higher permission to access this functionality" });
            // }

            //var result = await this.signInManager
            //   .PasswordSignInAsync(input.Email, input.Password, isPersistent: false, lockoutOnFailure: false);
            //this.Response.Cookies.Delete(".AspNetCore.Identity.Application");
            //this.Response.Headers.Remove("Set-Cookie");
            // FOR JWT not use PasswordSignInAsync but CheckPasswordSignInAsync - the second DO NOT SEND Cookie from
            // the Identiry server!
            var result = await this.signInManager
                .CheckPasswordSignInAsync(user, input.Password, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return this.BadRequest(new ErrorResponseModel
                {
                    Description = "Invalid login attempt",
                });
            }

            var response = await this.accountsService.Authenticate(user);

            //// the name of the cookie is given by the Framework, can be changed by the Identity Server post configuration
            // this.Response.Cookies.Append(GlobalConstants.JwtCookieName, response.AccessToken, new CookieOptions
            // {
            //    HttpOnly = true,
            //    Secure = true,
            //    // MaxAge = TimeSpan.FromTicks(response.ExpiresIn),
            //    // MaxAge = TimeSpan.FromTicks(60),
            //    // Expires = DateTimeOffset.UtcNow.AddMinutes(1),
            //    // SameSite = SameSiteMode.None,
            // });

            // return this.Ok(this.User.Identity.IsAuthenticated);
            return this.Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] UserRegisterRequestModel input)
        {
            // Not needed anymore since this logic is in a validation attribute GoogleReCaptchaValidationAttribute:
            // var recaptchaResult = await this.googleReCaptchaService.IsReCaptchaValid(input.Token);
            // if (!recaptchaResult)
            // {
            //     return this.BadRequest(new ErrorResponseModel
            //     {
            //         Description = "You failed the reCaptcha",
            //     });
            // }
            var request = this.HttpContext.Request.Form;

            if (input == null || !this.ModelState.IsValid)
            {
                return this.BadRequest(new ErrorResponseModel
                {
                    Description = "Invalid register attempt",
                });
            }

            if (input.Password != input.ConfirmPassword
                || string.IsNullOrWhiteSpace(input.Password)
                || string.IsNullOrWhiteSpace(input.ConfirmPassword))
            {
                return this.BadRequest(new ErrorResponseModel
                {
                    Description = "Passwords must match and should not be empty",
                });
            }

            var user = input.To<ApplicationUser>();

            // user.BlogLists.Add(new BlogList { IsSystem = true, Name = BlogListConstants.CurrentBlogListName });
            // user.BlogLists.Add(new BlogList { IsSystem = true, Name = BlogListConstants.LikesBlogListName });
            var result = await this.userManager.CreateAsync(user, input.Password);

            if (!result.Succeeded)
            {
                // foreach (var error in result.Errors)
                // {
                //     this.ModelState.AddModelError(string.Empty, error.Description);
                // }
                // return this.BadRequest(this.ModelState);
                return this.BadRequest(result.Errors
                                                .Select(e => new ErrorResponseModel
                                                {
                                                    Description = e.Description,
                                                })
                                                .FirstOrDefault());
            }

            await this.userManager.AddToRoleAsync(user, GlobalConstants.UserRoleName);

            // await this.signInManager.PasswordSignInAsync(input.Email, input.Password, isPersistent: false, lockoutOnFailure: false);
            return this.Ok(new UserRegisterResponseModel
            {
                Id = user.Id,
            });
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
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

            // delete JWT stored in the cookie:
            // this.Response.Cookies.Delete(GlobalConstants.JwtCookieName);
            // this method deletes the Cookies too:
            this.signInManager.SignOutAsync();

            return this.Ok();
        }
    }
}
