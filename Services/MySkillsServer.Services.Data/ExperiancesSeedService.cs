namespace MySkillsServer.Services.Data
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;

    using MySkillsServer.Data.Common.Repositories;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Data.Models;

    public class ExperiancesSeedService : IExperiancesSeedService
    {
        private readonly IRepository<Experiance> experiancesRepository;

        public ExperiancesSeedService(IRepository<Experiance> experiancesRepository)
        {
            this.experiancesRepository = experiancesRepository;
        }

        public async Task CreateAsync(ExperianceDTO experianceDTO)
        {
            // if no Job is provided
            if (experianceDTO.Job == null)
            {
                throw new ArgumentNullException(nameof(experianceDTO.Job));
            }

            var experiance = new Experiance
            {
                Job = experianceDTO.Job.Trim(),
                Company = experianceDTO.Company.Trim(),
                Url = experianceDTO.Url.Trim(),
                Logo = experianceDTO.Logo.Trim(),
                StartDate = DateTime.Parse(experianceDTO.StartDate, CultureInfo.InvariantCulture).Date,
                EndDate = DateTime.Parse(experianceDTO.EndDate, CultureInfo.InvariantCulture).Date,
                IconClassName = experianceDTO.IconClassName.Trim(),
                Details = experianceDTO.Details.Trim(),
            };

            await this.experiancesRepository.AddAsync(experiance);

            await this.experiancesRepository.SaveChangesAsync();
        }
    }
}
