namespace MySkillsServer.Web.ViewModels.ContactFormMessages
{
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Mapping;

    public class ContactFormMessageExportModel : IMapFrom<ContactFormMessage>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }
    }
}
