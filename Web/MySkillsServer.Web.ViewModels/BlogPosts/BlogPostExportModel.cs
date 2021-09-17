namespace MySkillsServer.Web.ViewModels.BlogPosts
{
    using System;
    using System.Collections.Generic;

    using AutoMapper;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Mapping;
    using MySkillsServer.Web.ViewModels.Categories;
    using MySkillsServer.Web.ViewModels.Comments;

    public class BlogPostExportModel : IMapFrom<BlogPost>
    {
        public string Id { get; set; }

        public string Author { get; set; }

        public string Title { get; set; }

        public string Details { get; set; }

        public DateTime PublishDate { get; set; }

        public string ImageRemoteFileUrl { get; set; }

        public string ImageFileExtension { get; set; }

        public string ExternalPostUrl { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int Likes { get; set; }

        public string UserEmail { get; set; }

        public ICollection<CommentExportModel> Comments { get; set; }

        public ICollection<CategoryExportModel> Categories { get; set; }
    }
}
