namespace MySkillsServer.Services.Data
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;

    using MySkillsServer.Data.Common.Repositories;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Data.Models;

    public class EducationsSeedService : IEducationsSeedService
    {
        private readonly IRepository<Education> educationsRepository;

        public EducationsSeedService(IRepository<Education> educationsRepository)
        {
            this.educationsRepository = educationsRepository;
        }

        public async Task CreateAsync(EducationDTO educationDTO)
        {
            // if no Degree is provided
            if (educationDTO.Degree == null)
            {
                throw new ArgumentNullException(nameof(educationDTO.Degree));
            }

            var education = new Education
            {
                Degree = educationDTO.Degree.Trim(),
                Speciality = educationDTO.Speciality.Trim(),
                Institution = educationDTO.Institution.Trim(),
                StartYear = int.Parse(educationDTO.StartYear.Trim()),
                EndYear = int.Parse(educationDTO.EndYear.Trim()),
                IconClassName = educationDTO.IconClassName.Trim(),
                Details = educationDTO.Details.Trim(),
            };

            await this.educationsRepository.AddAsync(education);

            await this.educationsRepository.SaveChangesAsync();
        }
    }
}
