namespace MySkillsServer.Web.ViewModels.Educations
{
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Mapping;

    public class EducationExportModel : IMapFrom<Education>
    {
        public string Degree { get; set; }

        public string Speciality { get; set; }

        public string Institution { get; set; }

        public int StartYear { get; set; }

        public int EndYear { get; set; }

        public string IconClassName { get; set; }

        public string Details { get; set; }

        public string UserId { get; set; }
    }
}
