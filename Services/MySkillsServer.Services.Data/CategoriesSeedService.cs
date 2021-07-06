namespace MySkillsServer.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using MySkillsServer.Data.Common.Repositories;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Data.Models;

    public class CategoriesSeedService : ICategoriesSeedService
    {
        private readonly IRepository<Category> categoriesRepository;

        public CategoriesSeedService(IRepository<Category> categoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
        }

        public async Task CreateAsync(CategoryDTO categoryDTO)
        {
            // if no Name is provided
            if (categoryDTO.Name == null)
            {
                throw new ArgumentNullException(nameof(categoryDTO.Name));
            }

            var category = new Category
            {
                Name = categoryDTO.Name.Trim(),
            };

            await this.categoriesRepository.AddAsync(category);

            await this.categoriesRepository.SaveChangesAsync();
        }
    }
}