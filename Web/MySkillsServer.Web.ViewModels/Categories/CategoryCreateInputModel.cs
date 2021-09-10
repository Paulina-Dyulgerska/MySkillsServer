namespace MySkillsServer.Web.ViewModels.Categories
{
    using System.ComponentModel.DataAnnotations;

    public class CategoryCreateInputModel
    {
        [Required]
        public string Name { get; set; }
    }
}
