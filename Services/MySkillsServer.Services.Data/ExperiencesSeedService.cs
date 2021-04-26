namespace MySkillsServer.Services.Data
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;

    using MySkillsServer.Data.Common.Repositories;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Data.Models;

    public class ExperiencesSeedService : IExperiencesSeedService
    {
        private readonly IRepository<Experience> experiencesRepository;

        public ExperiencesSeedService(IRepository<Experience> experiencesRepository)
        {
            this.experiencesRepository = experiencesRepository;
        }

        public async Task CreateAsync(ExperienceDTO experienceDTO)
        {
            // if no Job is provided
            if (experienceDTO.Job == null)
            {
                throw new ArgumentNullException(nameof(experienceDTO.Job));
            }

            var experience = new Experience
            {
                Job = experienceDTO.Job.Trim(),
                Company = experienceDTO.Company.Trim(),
                Url = experienceDTO.Url.Trim(),
                Logo = experienceDTO.Logo.Trim(),
                StartDate = DateTime.Parse(experienceDTO.StartDate.Trim(), CultureInfo.InvariantCulture).Date,
                EndDate = DateTime.Parse(experienceDTO.EndDate.Trim(), CultureInfo.InvariantCulture).Date,
                IconClassName = experienceDTO.IconClassName.Trim(),
                Details = experienceDTO.Details.Trim(),
            };

            await this.experiencesRepository.AddAsync(experience);

            await this.experiencesRepository.SaveChangesAsync();
        }
    }
}
