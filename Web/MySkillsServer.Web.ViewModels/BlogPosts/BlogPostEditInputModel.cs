namespace MySkillsServer.Web.ViewModels.BlogPosts
{
    using System.ComponentModel.DataAnnotations;

    public class BlogPostEditInputModel : BlogPostCreateInputModel
    {
        //[Required]
        public string Id { get; set; }

        public int Likes { get; set; }
    }
}
