namespace MySkillsServer.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MySkillsServer.Data.Common.Repositories;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Mapping;
    using MySkillsServer.Web.ViewModels.ContactFormMessages;

    public class ContactFormMessagesService : IContactFormMessagesService
    {
        private readonly IRepository<ContactFormMessage> contactFormMessagesRepository;

        public ContactFormMessagesService(IRepository<ContactFormMessage> contactFormMessagesRepository)
        {
            this.contactFormMessagesRepository = contactFormMessagesRepository;
        }

        public int GetCount()
        {
            return this.contactFormMessagesRepository.AllAsNoTracking().Count();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            return await this.contactFormMessagesRepository.All().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingAsync<T>()
        {
            return await this.contactFormMessagesRepository.AllAsNoTracking().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingOrderedAsync<T>()
        {
            return await this.contactFormMessagesRepository
                .AllAsNoTracking()
                .OrderBy(x => x.Id)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllOrderedAsPagesAsync<T>(string sortOrder, int page, int itemsPerPage)
        {
            return await this.contactFormMessagesRepository
                                .AllAsNoTracking()
                                .OrderBy(x => x.Id)
                                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                .To<T>()
                                .ToListAsync();
        }

        public async Task<T> GetByIdAsync<T>(int id)
        {
            return await this.contactFormMessagesRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();
        }

        public async Task<int> CreateAsync(ContactFormMessageCreateInputModel input, string userId)
        {
            var entity = input.To<ContactFormMessage>();
            entity.UserId = userId;

            await this.contactFormMessagesRepository.AddAsync(entity);

            await this.contactFormMessagesRepository.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<int> EditAsync(ContactFormMessageEditInputModel input, string userId)
        {
            var entity = await this.contactFormMessagesRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == input.Id);

            if (entity.UserId == userId)
            {
                entity.Name = input.Name.Trim();
                entity.Phone = input.Phone.Trim();
                entity.Subject = input.Subject.Trim();
                entity.Message = input.Message.Trim();
            }

            await this.contactFormMessagesRepository.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await this.contactFormMessagesRepository.All().FirstOrDefaultAsync(x => x.Id == id);
            this.contactFormMessagesRepository.Delete(entity);

            return await this.contactFormMessagesRepository.SaveChangesAsync();
        }
    }
}
