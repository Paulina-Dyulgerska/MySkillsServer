namespace MySkillsServer.Web.ViewModels.Accounts
{
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Mapping;
    using MySkillsServer.Web.Infrastructure.ValidationAttributes;

    public class UserRegisterRequestModel : IMapTo<ApplicationUser>, IHaveCustomMappings
    {
        [Required]
        [RegularExpression(
            @"^(?:[a-zA-Z0-9][a-zA-Z0-9_.-]+@(?:[a-zA-Z0-9-_]{2,}\.{1}[a-zA-Z0-9-_]{2,}))(?:\.[a-zA-Z0-9-_]{2,})?$",
            ErrorMessage = "Please enter a valid email.")]
        [EmailAddress]
        public string Email { get; set; }

        // [RegularExpression(@"^(?=.{8,})(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+!=]).*$")]
        [RegularExpression(
            @"^[a-zA-Z0-9]{6,}$",
            ErrorMessage = "Your password must be at least 6 characters long and contains only letters and numbers.")]
        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Password fields must match and not be empty.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        // [ModelBinder(Name = "token")]
        // [BindProperty(Name = "token")]
        [Required]
        [GoogleReCaptchaValidationAttribute]
        public string Token { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<UserRegisterRequestModel, ApplicationUser>()
                .ForMember(
                x => x.UserName,
                opt => opt.MapFrom(a => a.Email));
        }
    }
}
