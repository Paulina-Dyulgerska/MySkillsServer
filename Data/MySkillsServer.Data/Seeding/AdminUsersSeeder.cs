namespace MySkillsServer.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using MySkillsServer.Common;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Data.Models;

    public class AdminUsersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Users.Any())
            {
                return;
            }

            var jsonAdminUsers = File
                .ReadAllText("../../../MySkillsServer/Data/MySkillsServer.Data/Seeding/DataFiles/AdminUsers.json");
            var adminUsers = JsonSerializer.Deserialize<IEnumerable<AdminUserDTO>>(jsonAdminUsers);
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            foreach (var adminUser in adminUsers)
            {
                var user = new ApplicationUser
                {
                    UserName = adminUser.UserName,
                    Email = adminUser.Email,
                    EmailConfirmed = adminUser.EmailConfirmed,
                };

                var resultUserAdd = await userManager.CreateAsync(user, adminUser.Password);

                if (!resultUserAdd.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, resultUserAdd.Errors.Select(e => e.Description)));
                }

                var resultUserToRoleAdd = await userManager.AddToRolesAsync(
                    user,
                    new[] { GlobalConstants.AdministratorRoleName, GlobalConstants.UserRoleName });

                if (!resultUserToRoleAdd.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, resultUserAdd.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
