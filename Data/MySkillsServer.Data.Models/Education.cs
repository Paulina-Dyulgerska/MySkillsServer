namespace MySkillsServer.Data.Models
{
    using MySkillsServer.Data.Common.Models;

    public class Education : BaseModel<int>
    {
        public string Degree { get; set; }

        public string Speciality { get; set; }

        public string Institution { get; set; }

        public int StartYear { get; set; }

        public int EndYear { get; set; }

        public string IconClassName { get; set; }

        public string Details { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
