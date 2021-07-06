namespace MySkillsServer.Data.Models
{
    using MySkillsServer.Data.Common.Models;
    using System.ComponentModel.DataAnnotations.Schema;

    public class BlogPostCategory : BaseModel<int>
    {
        [ForeignKey(nameof(BlogPost))]
        public string BlogPostId { get; set; }

        public BlogPost BlogPost { get; set; }

        [ForeignKey(nameof(Category))]
        public string CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
