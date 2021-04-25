namespace MySkillsServer.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using MySkillsServer.Services.Data;
    using MySkillsServer.Services.Data.Models;

    public class ContactsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Contacts.Any())
            {
                return;
            }

            var jsonContacts = File
                .ReadAllText("../../../MySkillsServer/Data/MySkillsServer.Data/Seeding/DataFiles/Contacts.json");
            var contacts = JsonSerializer.Deserialize<IEnumerable<ContactDTO>>(jsonContacts);
            var contactsService = serviceProvider.GetRequiredService<IContactsSeedService>();

            foreach (var contact in contacts)
            {
                try
                {
                    await contactsService.CreateAsync(contact);
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
