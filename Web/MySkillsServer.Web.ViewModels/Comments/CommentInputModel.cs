namespace MySkillsServer.Web.ViewModels.Comments
{
    using System.ComponentModel.DataAnnotations;

    public class CommentInputModel
    {
        [Required]
        public string BlogPostId { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
