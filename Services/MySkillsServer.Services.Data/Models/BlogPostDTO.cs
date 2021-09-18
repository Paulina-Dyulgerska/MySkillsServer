namespace MySkillsServer.Services.Data.Models
{
    public class BlogPostDTO
    {
        public string Author { get; set; }

        public string Title { get; set; }

        public string Details { get; set; }

        public string ImageRemoteFileUrl { get; set; }

        public string ImageFileName { get; set; }

        public string ImageFileExtension { get; set; }

        public string ExternalPostUrl { get; set; }

        public string PublishDate { get; set; }

        public int Likes { get; set; }
    }
}
