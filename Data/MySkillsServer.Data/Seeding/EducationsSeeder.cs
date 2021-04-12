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

    public class EducationsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Educations.Any())
            {
                return;
            }

            var jsonEducations = File
                .ReadAllText("../../../MySkillsServer/Data/MySkillsServer.Data/Seeding/DataFiles/Educations.json");
            var educations = JsonSerializer.Deserialize<IEnumerable<EducationDTO>>(jsonEducations);
            var educationsService = serviceProvider.GetRequiredService<IEducationsSeedService>();

            foreach (var education in educations)
            {
                try
                {
                    await educationsService.CreateAsync(education);
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
