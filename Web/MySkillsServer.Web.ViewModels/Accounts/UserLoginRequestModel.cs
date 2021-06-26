namespace MySkillsServer.Web.ViewModels.Accounts
{
    using System.ComponentModel.DataAnnotations;

    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Mapping;
    using MySkillsServer.Web.Infrastructure.ValidationAttributes;

    public class UserLoginRequestModel : IMapTo<ApplicationUser>
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [GoogleReCaptchaValidationAttribute]
        public string Token { get; set; }
    }
}
