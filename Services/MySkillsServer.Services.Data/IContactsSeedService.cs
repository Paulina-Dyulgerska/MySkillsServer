namespace MySkillsServer.Services.Data
{
    using System.Threading.Tasks;

    using MySkillsServer.Services.Data.Models;

    public interface IContactsSeedService
    {
        public Task CreateAsync(ContactDTO contactDTO);
    }
}
