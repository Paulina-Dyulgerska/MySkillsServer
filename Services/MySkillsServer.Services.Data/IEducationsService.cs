namespace MySkillsServer.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MySkillsServer.Web.ViewModels.Educations;

    public interface IEducationsService
    {
        int GetCount();

        Task<IEnumerable<T>> GetAllAsync<T>();

        Task<IEnumerable<T>> GetAllAsNoTrackingAsync<T>();

        Task<IEnumerable<T>> GetAllAsNoTrackingOrderedAsync<T>();

        Task<IEnumerable<T>> GetOrderedAsPagesAsync<T>(string sortOrder, int page, int itemsPerPage);

        Task<T> GetByIdAsync<T>(int id);

        // Task CreateAsync(EducationCreateInputModel input, string userId);
        Task CreateAsync(EducationCreateInputModel input);

        Task EditAsync(EducationEditInputModel input);

        Task<int> DeleteAsync(int id);
    }
}
