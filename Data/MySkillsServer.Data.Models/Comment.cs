namespace MySkillsServer.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using MySkillsServer.Data.Common.Models;

    public class Comment : BaseModel<int>
    {
        [Required]
        public string BlogPostId { get; set; }

        public virtual BlogPost BlogPost { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int Likes { get; set; }
    }
}
