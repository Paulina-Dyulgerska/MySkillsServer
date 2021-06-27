namespace MySkillsServer.Web.ViewModels.Accounts
{
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Mapping;

    public class UserRegisterRequestModel : IMapTo<ApplicationUser>, IHaveCustomMappings
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        // [ModelBinder(Name = "token")]
        // [BindProperty(Name = "token")]
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
