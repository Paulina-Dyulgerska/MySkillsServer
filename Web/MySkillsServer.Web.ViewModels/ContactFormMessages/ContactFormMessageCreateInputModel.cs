namespace MySkillsServer.Web.ViewModels.ContactFormMessages
{
    using System.ComponentModel.DataAnnotations;

    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Mapping;
    using MySkillsServer.Web.Infrastructure.ValidationAttributes;

    public class ContactFormMessageCreateInputModel : IMapTo<ContactFormMessage>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        public string Phone { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }

        public string Ip { get; set; }

        [Required]
        [GoogleReCaptchaValidationAttribute]
        public string Token { get; set; }
    }
}
