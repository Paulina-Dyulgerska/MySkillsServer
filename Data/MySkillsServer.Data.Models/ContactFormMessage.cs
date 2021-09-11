namespace MySkillsServer.Data.Models
{
    using MySkillsServer.Data.Common.Models;

    public class ContactFormMessage : BaseModel<int>
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public string Ip { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
