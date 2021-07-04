namespace MySkillsServer.Web.ViewModels.ContactFormMessages
{
    using System.ComponentModel.DataAnnotations;

    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Mapping;

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
    }
}
