namespace MySkillsServer.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Web.ViewModels.ContactFormMessages;

    public interface IContactFormMessagesService
    {
        int GetCount();

        Task<IEnumerable<T>> GetAllAsync<T>();

        Task<IEnumerable<T>> GetAllAsNoTrackingAsync<T>();

        Task<IEnumerable<T>> GetAllAsNoTrackingOrderedAsync<T>();

        Task<IEnumerable<T>> GetOrderedAsPagesAsync<T>(string sortOrder, int page, int itemsPerPage);

        Task<T> GetByIdAsync<T>(int id);

        // Task<int> CreateAsync(ContactFormMessageCreateInputModel input);
        Task<int> CreateAsync(ContactFormMessageCreateInputModel input, ApplicationUser user);

        Task<int> EditAsync(ContactFormMessageEditInputModel input, ApplicationUser user);

        Task<int> DeleteAsync(int id);
    }
}
