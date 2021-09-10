namespace MySkillsServer.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using MySkillsServer.Data.Common.Models;

    public class ContactFormMessage : BaseModel<int>
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
