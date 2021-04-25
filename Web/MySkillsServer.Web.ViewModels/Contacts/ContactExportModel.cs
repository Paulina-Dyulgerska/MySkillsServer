namespace MySkillsServer.Web.ViewModels.Contacts
{
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Mapping;

    public class ContactExportModel : IMapFrom<Contact>
    {
        public int Id { get; set; }

        public string Icon { get; set; }

        public string Title { get; set; }

        public string Link { get; set; }

        public string LinkText { get; set; }
    }
}
