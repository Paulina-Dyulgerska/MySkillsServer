namespace MySkillsServer.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using MySkillsServer.Data.Common.Models;

    public class Education : BaseDeletableModel<int>
    {
        public string Degree { get; set; }

        public string Speciality { get; set; }

        public string Institution { get; set; }

        public int StartYear { get; set; }

        public int EndYear { get; set; }

        public string IconClassName { get; set; }

        public string Details { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
