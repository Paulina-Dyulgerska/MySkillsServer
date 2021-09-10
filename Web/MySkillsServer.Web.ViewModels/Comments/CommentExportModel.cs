namespace MySkillsServer.Web.ViewModels.Comments
{
    using AutoMapper;
    using MySkillsServer.Common;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Mapping;

    public class CommentExportModel : IMapFrom<Comment>, IHaveCustomMappings
    {
        public string Content { get; set; }

        public string PublishDate { get; set; }

        public string UserEmail { get; set; }

        public int Likes { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Comment, CommentExportModel>().ForMember(
                m => m.PublishDate,
                opt => opt.MapFrom(x => x.PublishDate.ToString(GlobalConstants.DateTimeFormat)));
        }
    }
}
