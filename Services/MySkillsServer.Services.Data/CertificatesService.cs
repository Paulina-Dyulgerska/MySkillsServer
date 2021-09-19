namespace MySkillsServer.Services.Data
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Azure.Storage.Blobs;
    using Microsoft.EntityFrameworkCore;
    using MySkillsServer.Common;
    using MySkillsServer.Data.Common.Repositories;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Mapping;
    using MySkillsServer.Web.ViewModels.Certificates;

    public class CertificatesService : ICertificatesService
    {
        private readonly IRepository<Certificate> certificatesRepository;
        private readonly BlobServiceClient blobServiceClient;

        public CertificatesService(
                        IRepository<Certificate> certificatesRepository,
                        BlobServiceClient blobServiceClient)
        {
            this.certificatesRepository = certificatesRepository;
            this.blobServiceClient = blobServiceClient;
        }

        public int GetCount()
        {
            return this.certificatesRepository.AllAsNoTracking().Count();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            return await this.certificatesRepository.All().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingAsync<T>()
        {
            return await this.certificatesRepository.AllAsNoTracking().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingOrderedAsync<T>()
        {
            return await this.certificatesRepository
                .AllAsNoTracking()
                .OrderBy(x => x.Id)
                .ThenByDescending(x => x.FileName)
                .ThenByDescending(x => x.FileExtension)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllOrderedAsPagesAsync<T>(string sortOrder, int page, int itemsPerPage)
        {
            return await this.certificatesRepository
                                .AllAsNoTracking()
                                .OrderByDescending(x => x.FileName)
                                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                .To<T>()
                                .ToListAsync();
        }

        public async Task<T> GetByIdAsync<T>(int id)
        {
            return await this.certificatesRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();
        }

        public async Task<int> CreateAsync(CertificateCreateInputModel input, string userId)
        {
            var extension = Path.GetExtension(input.InputFile.FileName).ToLower().TrimStart('.');
            var name = Path.GetFileNameWithoutExtension(input.InputFile.FileName);
            var fileName = Path.GetFileName(input.InputFile.FileName);
            var container = this.blobServiceClient.GetBlobContainerClient(GlobalConstants.AzureStorageBlobContainerNameCertificates);
            var blobClient = container.GetBlobClient(fileName);
            using var fileStream = input.InputFile.OpenReadStream();
            await blobClient.UploadAsync(fileStream);

            var entity = new Certificate
            {
                FileName = name,
                FileExtension = extension,
                RemoteFileUrl = blobClient.Uri.AbsoluteUri,
                UserId = userId,
            };

            await this.certificatesRepository.AddAsync(entity);

            await this.certificatesRepository.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<int> EditAsync(CertificateEditInputModel input, string userId)
        {
            var entity = await this.certificatesRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == input.Id);

            // entity.Title = input.Title.Trim();
            // entity.Author = input.Author.Trim();
            // entity.Details = input.Details.Trim();
            // entity.PublishDate = input.PublishDate;
            // entity.ExternalPostUrl = input.ExternalPostUrl;
            // entity.UserId = userId;
            // entity.Categories = null;
            // entity.Likes = input.Likes;
            PropertyInfo[] properties = entity.GetType().GetProperties(BindingFlags.Static | BindingFlags.Instance |
                            BindingFlags.Public | BindingFlags.NonPublic);

            // Stream fileStream = await this.UploadFileAsync(input, imageFilesDirectory, entity);
            await this.certificatesRepository.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<int> DeleteAsync(int id, string userId)
        {
            var entity = await this.certificatesRepository.All().FirstOrDefaultAsync(x => x.Id == id);
            entity.UserId = userId;

            this.certificatesRepository.Delete(entity);

            return await this.certificatesRepository.SaveChangesAsync();
        }
    }
}
