namespace MySkillsServer.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using MySkillsServer.Data.Common.Models;

    public class Category : BaseDeletableModel<string>
    {
        public Category()
        {
            this.Id = Guid.NewGuid().ToString();
            this.BlogPosts = new HashSet<BlogPostCategory>();
        }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<BlogPostCategory> BlogPosts { get; set; }
    }
}
