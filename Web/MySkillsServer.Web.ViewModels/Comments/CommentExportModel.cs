namespace MySkillsServer.Web.ViewModels.Comments
{
    using AutoMapper;
    using MySkillsServer.Common;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Mapping;
    using System;

    public class CommentExportModel : IMapFrom<Comment>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string BlogPostId { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string UserEmail { get; set; }

        public int Likes { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Comment, CommentExportModel>()
                //.ForMember(
                //    m => m.CreatedOn,
                //    opt => opt.MapFrom(x => x.CreatedOn.ToString(GlobalConstants.DateTimeFormat)))
                ;
        }
    }
}
