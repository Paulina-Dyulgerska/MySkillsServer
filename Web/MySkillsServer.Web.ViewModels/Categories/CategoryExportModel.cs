namespace MySkillsServer.Web.ViewModels.Categories
{
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Mapping;

    public class CategoryExportModel : IMapFrom<Category>, IMapFrom<BlogPostCategory>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
