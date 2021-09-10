namespace MySkillsServer.Services.Data
{
    using System.Threading.Tasks;

    using MySkillsServer.Web.ViewModels.Experiences;

    public interface IExperiencesService : IService<int>
    {
        Task<int> CreateAsync(ExperienceCreateInputModel input, string userId);

        Task<int> EditAsync(ExperienceEditInputModel input, string userId);

        Task<int> DeleteAsync(int id, string userId);
    }
}
