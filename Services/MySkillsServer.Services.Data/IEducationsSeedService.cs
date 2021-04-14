namespace MySkillsServer.Services.Data
{
    using System.Threading.Tasks;

    using MySkillsServer.Services.Data.Models;

    public interface IEducationsSeedService
    {
        public Task CreateAsync(EducationDTO educationDTO);
    }
}
