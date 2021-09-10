namespace MySkillsServer.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using MySkillsServer.Data.Common.Models;

    public class BlogPost : BaseDeletableModel<string>
    {
        public BlogPost()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Comments = new HashSet<Comment>();
            this.Categories = new HashSet<BlogPostCategory>();
        }

        [Required]
        public string Author { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Details { get; set; }

        // The content of the files is in the file system. What about the expernal storages?
        // If there is a file stored in the local system, I will find it by name, but if the file
        // is stoored on another external storage system, I will use the FileUrl as an locator.
        public string ImageRemoteFileUrl { get; set; }

        public string ImageFileName { get; set; }

        public string ImageFileExtension { get; set; }

        public string ExternalPostUrl { get; set; }

        public DateTime PublishDate { get; set; }

        public int Likes { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<BlogPostCategory> Categories { get; set; }
    }
}
