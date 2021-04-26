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

    public class ExperiencesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Experiences.Any())
            {
                return;
            }

            var jsonEducations = File
                .ReadAllText("../../../MySkillsServer/Data/MySkillsServer.Data/Seeding/DataFiles/Experiences.json");
            var experiences = JsonSerializer.Deserialize<IEnumerable<ExperienceDTO>>(jsonEducations);
            var experiencesService = serviceProvider.GetRequiredService<IExperiencesSeedService>();

            foreach (var experience in experiences)
            {
                try
                {
                    await experiencesService.CreateAsync(experience);
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
