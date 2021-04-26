namespace MySkillsServer.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MySkillsServer.Web.ViewModels.Experiences;

    public interface IExperiencesService
    {
        int GetCount();

        Task<IEnumerable<T>> GetAllAsync<T>();

        Task<IEnumerable<T>> GetAllAsNoTrackingAsync<T>();

        Task<IEnumerable<T>> GetAllAsNoTrackingOrderedAsync<T>();

        Task<IEnumerable<T>> GetOrderedAsPagesAsync<T>(string sortOrder, int page, int itemsPerPage);

        Task<T> GetByIdAsync<T>(int id);

        // Task CreateAsync(ExperienceCreateInputModel input, string userId);
        Task<int> CreateAsync(ExperienceCreateInputModel input);

        Task<int> EditAsync(ExperienceEditInputModel input);

        Task<int> DeleteAsync(int id);
    }
}
