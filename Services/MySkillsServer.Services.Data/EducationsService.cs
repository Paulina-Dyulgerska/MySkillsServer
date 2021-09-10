namespace MySkillsServer.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MySkillsServer.Data.Common.Repositories;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Mapping;
    using MySkillsServer.Web.ViewModels.Educations;

    public class EducationsService : IEducationsService
    {
        private readonly IRepository<Education> educationsRepository;

        public EducationsService(IRepository<Education> educationsRepository)
        {
            this.educationsRepository = educationsRepository;
        }

        public int GetCount()
        {
            return this.educationsRepository.AllAsNoTracking().Count();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            return await this.educationsRepository.All().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingAsync<T>()
        {
            return await this.educationsRepository.AllAsNoTracking().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingOrderedAsync<T>()
        {
            return await this.educationsRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.EndYear)
                .ThenBy(x => x.Speciality)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllOrderedAsPagesAsync<T>(string sortOrder, int page, int itemsPerPage)
        {
            return await this.educationsRepository
                                .AllAsNoTracking()
                                .OrderByDescending(x => x.EndYear)
                                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                .To<T>()
                                .ToListAsync();
        }

        public async Task<T> GetByIdAsync<T>(int id)
        {
            return await this.educationsRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();
        }

        public async Task<int> CreateAsync(EducationCreateInputModel input, string userId)
        {
            // var userEntity = this.usersRepository.AllAsNoTracking()
            //   .FirstOrDefault(x => x.UserName == articleInputModel.UserId);
            //// take the user and record its id in the article, product, conformity, etc.
            var entity = new Education
            {
                Degree = input.Degree.Trim(),
                Speciality = input.Speciality.Trim(),
                Institution = input.Institution.Trim(),
                StartYear = input.StartYear,
                EndYear = input.EndYear,
                IconClassName = input.IconClassName.Trim(),
                Details = input.Details.Trim(),
                UserId = userId,
            };

            await this.educationsRepository.AddAsync(entity);

            await this.educationsRepository.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<int> EditAsync(EducationEditInputModel input, string userId)
        {
            var entity = await this.educationsRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == input.Id);

            // var userEntity = this.usersRepository.AllAsNoTracking()
            //    .FirstOrDefault(x => x.UserName == articleInputModel.UserId);
            // take the user and record its id in the article, product, conformity, etc.\\
            entity.Degree = input.Degree.Trim();
            entity.Speciality = input.Speciality.Trim();
            entity.Institution = input.Institution.Trim();
            entity.StartYear = input.StartYear;
            entity.EndYear = input.EndYear;
            entity.IconClassName = input.IconClassName.Trim();
            entity.Details = input.Details.Trim();
            entity.UserId = userId;

            await this.educationsRepository.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<int> DeleteAsync(int id, string userId)
        {
            var entity = await this.educationsRepository.All().FirstOrDefaultAsync(x => x.Id == id);
            entity.UserId = userId;

            this.educationsRepository.Delete(entity);

            return await this.educationsRepository.SaveChangesAsync();
        }
    }
}
