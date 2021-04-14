namespace MySkillsServer.Services.Data
{
    using System.Threading.Tasks;

    using MySkillsServer.Services.Data.Models;

    public interface IExperiancesSeedService
    {
        public Task CreateAsync(ExperianceDTO experianceDTO);
    }
}
