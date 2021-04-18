﻿namespace MySkillsServer.Web.ViewModels.Experiances
{
    using System;

    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Mapping;

    public class ExperianceExportModel : IMapFrom<Experiance>
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public string Logo { get; set; }

        public string Company { get; set; }

        public string Job { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string IconClassName { get; set; }

        public string Details { get; set; }

        public string UserId { get; set; }
    }
}
