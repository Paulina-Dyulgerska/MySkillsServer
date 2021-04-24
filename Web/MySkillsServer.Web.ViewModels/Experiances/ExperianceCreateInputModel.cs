namespace MySkillsServer.Web.ViewModels.Experiances
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using MySkillsServer.Common.ValidationAttributes;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Mapping;

    public class ExperianceCreateInputModel : IMapTo<Experiance>
    {
        public string Url { get; set; }

        public string Logo { get; set; }

        public string Company { get; set; }

        public string Job { get; set; }

        [DataType(DataType.Date)]
        [DateAttribute]
        [Required]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [DateAttribute]
        public DateTime? EndDate { get; set; }

        public string IconClassName { get; set; }

        public string Details { get; set; }

        public string UserId { get; set; }
    }
}
