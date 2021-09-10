namespace MySkillsServer.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using MySkillsServer.Common;
    using MySkillsServer.Data.Common.Repositories;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Data.Models;

    public class ContactsSeedService : IContactsSeedService
    {
        private readonly IRepository<Contact> contactsRepository;
        private readonly IRepository<ApplicationUser> users;
        private readonly IRepository<ApplicationRole> roles;

        public ContactsSeedService(
            IRepository<Contact> contactsRepository,
            IRepository<ApplicationUser> users,
            IRepository<ApplicationRole> roles)
        {
            this.contactsRepository = contactsRepository;
            this.users = users;
            this.roles = roles;
        }

        public async Task CreateAsync(ContactDTO contactDTO)
        {
            // if no Degree is provided
            if (contactDTO.LinkText == null)
            {
                throw new ArgumentNullException(nameof(contactDTO.LinkText));
            }

            var adminRoleId = this.roles.AllAsNoTracking().FirstOrDefault(x => x.Name == GlobalConstants.AdministratorRoleName).Id;
            var user = this.users.All().FirstOrDefault(x => x.Roles.Any(x => x.RoleId == adminRoleId));

            var contact = new Contact
            {
                Icon = contactDTO.Icon.Trim(),
                Title = contactDTO.Title.Trim(),
                Link = contactDTO.Link.Trim(),
                LinkText = contactDTO.LinkText.Trim(),
                UserId = user.Id,
            };

            await this.contactsRepository.AddAsync(contact);

            await this.contactsRepository.SaveChangesAsync();
        }
    }
}
