namespace MySkillsServer.Web.ViewModels.Contacts
{
    using System.ComponentModel.DataAnnotations;

    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Mapping;

    public class ContactCreateInputModel : IMapTo<Contact>
     {
        [Required]
        public string Icon { get; set; }

        [Required]
        public string Title { get; set; }

        public string Link { get; set; }

        [Required]
        public string LinkText { get; set; }
    }
}
