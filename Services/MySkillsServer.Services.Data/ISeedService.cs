namespace MySkillsServer.Services.Data
{
    using System.Threading.Tasks;

    public interface ISeedService<T>
    {
        public Task CreateAsync(T seededTypeDTO);
    }
}
