namespace MySkillsServer.Web.ViewModels.Educations
{
    using System.ComponentModel.DataAnnotations;

    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Mapping;

    public class EducationCreateInputModel : IMapTo<Education>
    {
        [Required]
        public string Degree { get; set; }

        [Required]
        public string Speciality { get; set; }

        [Required]
        public string Institution { get; set; }

        // [Range(1950, 2021)]
        public int StartYear { get; set; }

        // [Range(1950, 2021)]
        public int EndYear { get; set; }

        public string IconClassName { get; set; }

        public string Details { get; set; }

        public string UserId { get; set; }
    }
}
