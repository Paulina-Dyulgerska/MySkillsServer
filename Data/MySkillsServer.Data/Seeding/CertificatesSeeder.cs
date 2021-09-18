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

    public class CertificatesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Certificates.Any())
            {
                return;
            }

            var jsonCertificates = File
                .ReadAllText("../../../MySkillsServer/Data/MySkillsServer.Data/Seeding/DataFiles/Certificates.json");
            var certificates = JsonSerializer.Deserialize<IEnumerable<CertificateDTO>>(jsonCertificates);
            var certificatesSeedService = serviceProvider.GetRequiredService<ICertificatesSeedService>();

            foreach (var certificate in certificates)
            {
                try
                {
                    await certificatesSeedService.CreateAsync(certificate);
                }
                catch (Exception ex)
                {
                    var a = ex;
                }
            }
        }
    }
}
