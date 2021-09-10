namespace MySkillsServer.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MySkillsServer.Data.Common.Repositories;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Mapping;
    using MySkillsServer.Web.ViewModels.Experiences;

    public class ExperiencesService : IExperiencesService
    {
        private readonly IRepository<Experience> experiencesRepository;

        public ExperiencesService(IRepository<Experience> experiencesRepository)
        {
            this.experiencesRepository = experiencesRepository;
        }

        public int GetCount()
        {
            return this.experiencesRepository.AllAsNoTracking().Count();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            return await this.experiencesRepository.All().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingAsync<T>()
        {
            return await this.experiencesRepository.AllAsNoTracking().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingOrderedAsync<T>()
        {
            return await this.experiencesRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.EndDate)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllOrderedAsPagesAsync<T>(string sortOrder, int page, int itemsPerPage)
        {
            return await this.experiencesRepository
                                .AllAsNoTracking()
                                .OrderByDescending(x => x.EndDate)
                                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                .To<T>()
                                .ToListAsync();
        }

        public async Task<T> GetByIdAsync<T>(int id)
        {
            return await this.experiencesRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();
        }

        public async Task<int> CreateAsync(ExperienceCreateInputModel input, string userId)
        {
            // var userEntity = this.usersRepository.AllAsNoTracking()
            //   .FirstOrDefault(x => x.UserName == articleInputModel.UserId);
            //// take the user and record its id in the article, product, conformity, etc.
            var entity = new Experience();

            this.InstanceBuilder(input, entity, userId);

            await this.experiencesRepository.AddAsync(entity);

            await this.experiencesRepository.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<int> EditAsync(ExperienceEditInputModel input, string userId)
        {
            var entity = await this.experiencesRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == input.Id);

            this.InstanceBuilder(input, entity, userId);

            await this.experiencesRepository.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<int> DeleteAsync(int id, string userId)
        {
            var entity = await this.experiencesRepository.All().FirstOrDefaultAsync(x => x.Id == id);
            entity.UserId = userId;

            this.experiencesRepository.Delete(entity);

            return await this.experiencesRepository.SaveChangesAsync();
        }

        private void InstanceBuilder(ExperienceCreateInputModel input, Experience entity, string userId)
        {
            entity.Url = input.Url.Trim();
            entity.Logo = input.Logo.Trim();
            entity.Company = input.Company.Trim();
            entity.Job = input.Job;
            entity.StartDate = input.StartDate.ToUniversalTime();
            entity.IconClassName = input.IconClassName.Trim();
            entity.Details = input.Details.Trim();
            entity.UserId = userId;

            this.EndDateParser(input, entity);
        }

        private void EndDateParser(ExperienceCreateInputModel input, Experience entity)
        {
            if (input.EndDate != null)
            {
                entity.EndDate = input.EndDate?.ToUniversalTime();
            }
            else
            {
                entity.EndDate = DateTime.UtcNow;
            }
        }
    }
}
