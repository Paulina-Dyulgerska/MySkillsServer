namespace MySkillsServer.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using MySkillsServer.Services.Data;
    using MySkillsServer.Services.Data.Models;

    public class BlogPostsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.BlogPosts.Any())
            {
                return;
            }

            var jsonBlogPosts = File
                .ReadAllText("../../../MySkillsServer/Data/MySkillsServer.Data/Seeding/DataFiles/BlogPosts.json");
            var blogPosts = JsonSerializer.Deserialize<IEnumerable<BlogPostDTO>>(jsonBlogPosts);
            var blogPostsSeedService = serviceProvider.GetRequiredService<IBlogPostSeedService>();

            foreach (var blogPost in blogPosts)
            {
                try
                {
                    await blogPostsSeedService.CreateAsync(blogPost);
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
