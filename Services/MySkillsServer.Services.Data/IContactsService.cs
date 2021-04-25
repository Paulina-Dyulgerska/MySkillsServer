namespace MySkillsServer.Services.Data
{
    using MySkillsServer.Web.ViewModels.Contacts;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IContactsService
    {
        int GetCount();

        Task<IEnumerable<T>> GetAllAsync<T>();

        Task<IEnumerable<T>> GetAllAsNoTrackingAsync<T>();

        Task<IEnumerable<T>> GetAllAsNoTrackingOrderedAsync<T>();

        Task<IEnumerable<T>> GetOrderedAsPagesAsync<T>(string sortOrder, int page, int itemsPerPage);

        Task<T> GetByIdAsync<T>(int id);

        // Task CreateAsync(ContactCreateInputModel input, string userId);
        Task<int> CreateAsync(ContactCreateInputModel input);

        Task<int> EditAsync(ContactEditInputModel input);

        Task<int> DeleteAsync(int id);
    }
}
