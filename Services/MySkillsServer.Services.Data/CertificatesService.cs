namespace MySkillsServer.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using MySkillsServer.Data.Models;
    using MySkillsServer.Web.ViewModels.Certificates;

    public class CertificatesService : ICertificatesService
    {
        public Task<string> CreateAsync(CertificateCreateInputModel input, string userId, string certificatesFilesDirectory)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(string id, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<string> EditAsync(CertificateEditInputModel input, string userId, string certificatesFilesDirectory)
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

        public Task<T> GetByIdAsync<T>(string id)
        {
            throw new NotImplementedException();
        }

        public int GetCount()
        {
            throw new NotImplementedException();
        }
    }
}
