namespace MySkillsServer.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using MySkillsServer.Data.Common.Repositories;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Data.Models;

    public class ContactsSeedService : IContactsSeedService
    {
        private readonly IRepository<Contact> contactsRepository;

        public ContactsSeedService(IRepository<Contact> contactsRepository)
        {
            this.contactsRepository = contactsRepository;
        }

        public async Task CreateAsync(ContactDTO contactDTO)
        {
            // if no Degree is provided
            if (contactDTO.LinkText == null)
            {
                throw new ArgumentNullException(nameof(contactDTO.LinkText));
            }

            var contact = new Contact
            {
                Icon = contactDTO.Icon.Trim(),
                Title = contactDTO.Title.Trim(),
                Link = contactDTO.Link.Trim(),
                LinkText = contactDTO.LinkText.Trim(),
            };

            await this.contactsRepository.AddAsync(contact);

            await this.contactsRepository.SaveChangesAsync();
        }
    }
}
