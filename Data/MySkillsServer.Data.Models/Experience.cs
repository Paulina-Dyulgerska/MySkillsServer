namespace MySkillsServer.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using MySkillsServer.Data.Common.Models;

    public class Experience : BaseDeletableModel<int>
    {
        public string Url { get; set; }

        public string Logo { get; set; }

        public string Company { get; set; }

        public string Job { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string IconClassName { get; set; }

        public string Details { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
