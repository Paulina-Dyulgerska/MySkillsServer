namespace MySkillsServer.Web.ViewModels.BlogPosts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    public class BlogPostCreateInputModel
    {
        //[Required]
        public string Title { get; set; }

        //[Required]
        public string Author { get; set; }

        //[Required]
        public string Details { get; set; }

        public IFormFile InputFile { get; set; }

        public string ExternalPostUrl { get; set; }

        public DateTime PublishDate { get; set; }

        public IEnumerable<int> Categories { get; set; }
    }
}
