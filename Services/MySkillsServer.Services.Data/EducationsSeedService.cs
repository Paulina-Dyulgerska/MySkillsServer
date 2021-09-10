namespace MySkillsServer.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using MySkillsServer.Common;
    using MySkillsServer.Data.Common.Repositories;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Data.Models;

    public class EducationsSeedService : IEducationsSeedService
    {
        private readonly IRepository<Education> educationsRepository;
        private readonly IRepository<ApplicationUser> users;
        private readonly IRepository<ApplicationRole> roles;

        public EducationsSeedService(
            IRepository<Education> educationsRepository,
            IRepository<ApplicationUser> users,
            IRepository<ApplicationRole> roles)
        {
            this.educationsRepository = educationsRepository;
            this.users = users;
            this.roles = roles;
        }

        public async Task CreateAsync(EducationDTO educationDTO)
        {
            // if no Degree is provided
            if (educationDTO.Degree == null)
            {
                throw new ArgumentNullException(nameof(educationDTO.Degree));
            }

            var adminRoleId = this.roles.AllAsNoTracking().FirstOrDefault(x => x.Name == GlobalConstants.AdministratorRoleName).Id;
            var user = this.users.All().FirstOrDefault(x => x.Roles.Any(x => x.RoleId == adminRoleId));

            var education = new Education
            {
                Degree = educationDTO.Degree.Trim(),
                Speciality = educationDTO.Speciality.Trim(),
                Institution = educationDTO.Institution.Trim(),
                StartYear = int.Parse(educationDTO.StartYear.Trim()),
                EndYear = int.Parse(educationDTO.EndYear.Trim()),
                IconClassName = educationDTO.IconClassName.Trim(),
                Details = educationDTO.Details.Trim(),
                UserId = user.Id,
            };

            await this.educationsRepository.AddAsync(education);

            await this.educationsRepository.SaveChangesAsync();
        }
    }
}
