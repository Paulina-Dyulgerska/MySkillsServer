namespace MySkillsServer.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    using MySkillsServer.Data.Common.Models;

    public class BlogPostCategory : BaseModel<int>
    {
        [ForeignKey(nameof(BlogPost))]
        public string BlogPostId { get; set; }

        public BlogPost BlogPost { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
