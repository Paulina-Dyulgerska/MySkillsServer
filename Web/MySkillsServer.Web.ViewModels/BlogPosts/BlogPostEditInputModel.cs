namespace MySkillsServer.Web.ViewModels.BlogPosts
{
    public class BlogPostEditInputModel : BlogPostCreateInputModel
    {
        // [Required]
        public string Id { get; set; }

        public int Likes { get; set; }
    }
}
