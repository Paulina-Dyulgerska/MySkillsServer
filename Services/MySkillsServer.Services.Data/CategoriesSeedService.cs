namespace MySkillsServer.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using MySkillsServer.Common;
    using MySkillsServer.Data.Common.Repositories;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Data.Models;

    public class CategoriesSeedService : ICategoriesSeedService
    {
        private readonly IRepository<Category> categoriesRepository;
        private readonly IRepository<ApplicationUser> users;
        private readonly IRepository<ApplicationRole> roles;

        public CategoriesSeedService(
            IRepository<Category> categoriesRepository,
            IRepository<ApplicationUser> users,
            IRepository<ApplicationRole> roles)
        {
            this.categoriesRepository = categoriesRepository;
            this.users = users;
            this.roles = roles;
        }

        public async Task CreateAsync(CategoryDTO categoryDTO)
        {
            // if no Name is provided
            if (categoryDTO.Name == null)
            {
                throw new ArgumentNullException(nameof(categoryDTO.Name));
            }

            var adminRoleId = this.roles.AllAsNoTracking().FirstOrDefault(x => x.Name == GlobalConstants.AdministratorRoleName).Id;
            var user = this.users.All().FirstOrDefault(x => x.Roles.Any(x => x.RoleId == adminRoleId));

            var category = new Category
            {
                Name = categoryDTO.Name.Trim(),
                UserId = user.Id,
            };

            await this.categoriesRepository.AddAsync(category);

            await this.categoriesRepository.SaveChangesAsync();
        }
    }
}
