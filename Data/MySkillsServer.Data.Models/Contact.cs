namespace MySkillsServer.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using MySkillsServer.Data.Common.Models;

    public class Contact : BaseDeletableModel<int>
    {
        public string Icon { get; set; }

        public string Title { get; set; }

        public string Link { get; set; }

        public string LinkText { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
