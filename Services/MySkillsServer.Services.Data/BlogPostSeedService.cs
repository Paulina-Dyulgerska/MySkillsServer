namespace MySkillsServer.Services.Data
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using MySkillsServer.Common;
    using MySkillsServer.Data.Common.Repositories;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Data.Models;

    public class BlogPostSeedService : IBlogPostSeedService
    {
        private readonly IRepository<BlogPost> blogPostsRepository;
        private readonly IRepository<ApplicationUser> users;
        private readonly IRepository<ApplicationRole> roles;

        public BlogPostSeedService(
            IRepository<BlogPost> blogPostsRepository,
            IRepository<ApplicationUser> users,
            IRepository<ApplicationRole> roles)
        {
            this.blogPostsRepository = blogPostsRepository;
            this.users = users;
            this.roles = roles;
        }

        public async Task CreateAsync(BlogPostDTO blogPostDTO)
        {
            // if no Title is provided
            if (blogPostDTO.Title == null)
            {
                throw new ArgumentNullException(nameof(blogPostDTO.Title));
            }

            var adminRoleId = this.roles.AllAsNoTracking().FirstOrDefault(x => x.Name == GlobalConstants.AdministratorRoleName).Id;
            var user = this.users.All().FirstOrDefault(x => x.Roles.Any(x => x.RoleId == adminRoleId));

            var blogPost = new BlogPost
            {
                Author = blogPostDTO.Author,
                Title = blogPostDTO.Title,
                Details = blogPostDTO.Details,
                ImageRemoteFileUrl = blogPostDTO.ImageRemoteFileUrl,
                ImageFileExtension = blogPostDTO.ImageFileExtension,
                ExternalPostUrl = blogPostDTO.ExternalPostUrl,
                Likes = blogPostDTO.Likes,
                PublishDate = DateTime.Parse(blogPostDTO.PublishDate.Trim(), CultureInfo.InvariantCulture).Date,
                User = user,
            };

            await this.blogPostsRepository.AddAsync(blogPost);
            blogPost.ImageRemoteFileUrl = blogPost.Id;

            await this.blogPostsRepository.SaveChangesAsync();
        }
    }
}
