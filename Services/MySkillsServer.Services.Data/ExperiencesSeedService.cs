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

    public class ExperiencesSeedService : IExperiencesSeedService
    {
        private readonly IRepository<Experience> experiencesRepository;
        private readonly IRepository<ApplicationUser> users;
        private readonly IRepository<ApplicationRole> roles;

        public ExperiencesSeedService(
            IRepository<Experience> experiencesRepository,
            IRepository<ApplicationUser> users,
            IRepository<ApplicationRole> roles)
        {
            this.experiencesRepository = experiencesRepository;
            this.users = users;
            this.roles = roles;
        }

        public async Task CreateAsync(ExperienceDTO experienceDTO)
        {
            // if no Job is provided
            if (experienceDTO.Job == null)
            {
                throw new ArgumentNullException(nameof(experienceDTO.Job));
            }

            var adminRoleId = this.roles.AllAsNoTracking().FirstOrDefault(x => x.Name == GlobalConstants.AdministratorRoleName).Id;
            var user = this.users.All().FirstOrDefault(x => x.Roles.Any(x => x.RoleId == adminRoleId));

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
                UserId = user.Id,
            };

            await this.experiencesRepository.AddAsync(experience);

            await this.experiencesRepository.SaveChangesAsync();
        }
    }
}
