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

    public class CategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Categories.Any())
            {
                return;
            }

            var jsonCategories = File
                .ReadAllText("../../../MySkillsServer/Data/MySkillsServer.Data/Seeding/DataFiles/Categories.json");
            var categories = JsonSerializer.Deserialize<IEnumerable<CategoryDTO>>(jsonCategories);
            var categoriesSeedService = serviceProvider.GetRequiredService<ICategoriesSeedService>();

            foreach (var category in categories)
            {
                try
                {
                    await categoriesSeedService.CreateAsync(category);
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
