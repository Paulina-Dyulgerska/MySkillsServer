namespace MySkillsServer.Web.ViewModels.Educations
{
    using System.ComponentModel.DataAnnotations;

    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Mapping;
    using MySkillsServer.Web.Infrastructure.ValidationAttributes;

    public class EducationCreateInputModel : IMapTo<Education>
    {
        [Required]
        [MinLength(2)]
        public string Degree { get; set; }

        [Required]
        [MinLength(5)]
        public string Speciality { get; set; }

        [Required]
        [MinLength(5)]
        public string Institution { get; set; }

        [YearValidationAttribute(1950)]
        public int StartYear { get; set; }

        [YearValidationAttribute(1950)]
        public int EndYear { get; set; }

        public string IconClassName { get; set; }

        public string Details { get; set; }

        public string UserId { get; set; }
    }
}
