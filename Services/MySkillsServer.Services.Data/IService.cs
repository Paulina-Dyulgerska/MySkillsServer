namespace MySkillsServer.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IService<TKey>
    {
        int GetCount();

        Task<IEnumerable<T>> GetAllAsync<T>();

        Task<IEnumerable<T>> GetAllAsNoTrackingAsync<T>();

        Task<IEnumerable<T>> GetAllAsNoTrackingOrderedAsync<T>();

        Task<IEnumerable<T>> GetAllOrderedAsPagesAsync<T>(string sortOrder, int page, int itemsPerPage);

        Task<T> GetByIdAsync<T>(TKey id);
    }
}
