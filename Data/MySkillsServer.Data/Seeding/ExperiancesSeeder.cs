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

    public class ExperiancesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Experiances.Any())
            {
                return;
            }

            var jsonEducations = File
                .ReadAllText("../../../MySkillsServer/Data/MySkillsServer.Data/Seeding/DataFiles/Experiances.json");
            var experiances = JsonSerializer.Deserialize<IEnumerable<ExperianceDTO>>(jsonEducations);
            var experiancesService = serviceProvider.GetRequiredService<IExperiancesSeedService>();

            foreach (var experiance in experiances)
            {
                try
                {
                    await experiancesService.CreateAsync(experiance);
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
