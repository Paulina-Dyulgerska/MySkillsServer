namespace MySkillsServer.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MySkillsServer.Web.ViewModels.Categories;

    public class CategoriesService : ICategoriesService
    {
        public Task<int> CreateAsync(CategoryCreateInputModel input, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int id, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<int> EditAsync(CategoryEditInputModel input, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsNoTrackingAsync<T>()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsNoTrackingOrderedAsync<T>()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsync<T>()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllOrderedAsPagesAsync<T>(string sortOrder, int page, int itemsPerPage)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetByIdAsync<T>(int id)
        {
            throw new NotImplementedException();
        }

        public int GetCount()
        {
            throw new NotImplementedException();
        }
    }
}
