namespace MySkillsServer.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MySkillsServer.Data.Common.Repositories;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Mapping;
    using MySkillsServer.Web.ViewModels.Contacts;

    public class ContactsService : IContactsService
    {
        private readonly IRepository<Contact> contactsRepository;

        public ContactsService(IRepository<Contact> contactsRepository)
        {
            this.contactsRepository = contactsRepository;
        }

        public int GetCount()
        {
            return this.contactsRepository.AllAsNoTracking().Count();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            return await this.contactsRepository.All().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingAsync<T>()
        {
            return await this.contactsRepository.AllAsNoTracking().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingOrderedAsync<T>()
        {
            return await this.contactsRepository
                .AllAsNoTracking()
                .OrderBy(x => x.Id)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetOrderedAsPagesAsync<T>(string sortOrder, int page, int itemsPerPage)
        {
            return await this.contactsRepository
                                .AllAsNoTracking()
                                .OrderBy(x => x.Id)
                                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                .To<T>()
                                .ToListAsync();
        }

        public async Task<T> GetByIdAsync<T>(int id)
        {
            return await this.contactsRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();
        }

        // public async Task CreateAsync(ContactCreateInputModel input, string userId)
        public async Task<int> CreateAsync(ContactCreateInputModel input)
        {
            // var userEntity = this.usersRepository.AllAsNoTracking()
            //   .FirstOrDefault(x => x.UserName == articleInputModel.UserId);
            //// take the user and record its id in the article, product, conformity, etc.
            // var entity = new Contact
            // {
            //    Icon = input.Icon.Trim(),
            //    Title = input.Title.Trim(),
            //    Link = input.Link.Trim(),
            //    LinkText = input.LinkText.Trim(),
            // };
            var entity = input.To<Contact>();

            await this.contactsRepository.AddAsync(entity);

            await this.contactsRepository.SaveChangesAsync();

            return entity.Id;
        }

        // public async Task EditAsync(EducationEditInputModel input, string userId)
        public async Task<int> EditAsync(ContactEditInputModel input)
        {
            var entity = await this.contactsRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == input.Id);

            // var userEntity = this.usersRepository.AllAsNoTracking()
            //    .FirstOrDefault(x => x.UserName == articleInputModel.UserId);
            // take the user and record its id in the article, product, conformity, etc.\\
            entity.Icon = input.Icon.Trim();
            entity.Title = input.Title.Trim();
            entity.Link = input.Link.Trim();
            entity.LinkText = input.LinkText.Trim();

            await this.contactsRepository.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await this.contactsRepository.All().FirstOrDefaultAsync(x => x.Id == id);
            this.contactsRepository.Delete(entity);

            return await this.contactsRepository.SaveChangesAsync();
        }

        private string PascalCaseConverterWords(string stringToFix)
        {
            var st = new StringBuilder();
            var wordsInStringToFix = stringToFix.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in wordsInStringToFix)
            {
                st.Append(char.ToUpper(word[0]));

                for (int i = 1; i < word.Length; i++)
                {
                    st.Append(char.ToLower(word[i]));
                }

                st.Append(' ');
            }

            return st.ToString().Trim();
        }
    }
}
