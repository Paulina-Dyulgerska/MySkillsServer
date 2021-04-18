namespace MySkillsServer.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MySkillsServer.Data.Common.Repositories;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Mapping;
    using MySkillsServer.Web.ViewModels.Experiances;

    public class ExperiancesService : IExperiancesService
    {
        private readonly IRepository<Experiance> experiancesRepository;

        public ExperiancesService(IRepository<Experiance> experiancesRepository)
        {
            this.experiancesRepository = experiancesRepository;
        }

        public int GetCount()
        {
            return this.experiancesRepository.AllAsNoTracking().Count();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            return await this.experiancesRepository.All().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingAsync<T>()
        {
            return await this.experiancesRepository.AllAsNoTracking().To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingOrderedAsync<T>()
        {
            return await this.experiancesRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.EndDate)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetOrderedAsPagesAsync<T>(string sortOrder, int page, int itemsPerPage)
        {
            return await this.experiancesRepository
                                .AllAsNoTracking()
                                .OrderByDescending(x => x.EndDate)
                                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                                .To<T>()
                                .ToListAsync();
        }

        public async Task<T> GetByIdAsync<T>(int id)
        {
            return await this.experiancesRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();
        }

        // public async Task CreateAsync(EducationCreateInputModel input, string userId)
        public async Task<int> CreateAsync(ExperianceCreateInputModel input)
        {
            // var userEntity = this.usersRepository.AllAsNoTracking()
            //   .FirstOrDefault(x => x.UserName == articleInputModel.UserId);
            //// take the user and record its id in the article, product, conformity, etc.
            var entity = new Experiance();

            this.InstanceBuilder(input, entity);

            await this.experiancesRepository.AddAsync(entity);

            await this.experiancesRepository.SaveChangesAsync();

            return entity.Id;
        }

        // public async Task EditAsync(EducationEditInputModel input, string userId)
        public async Task<int> EditAsync(ExperianceEditInputModel input)
        {
            var entity = await this.experiancesRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == input.Id);

            // var userEntity = this.usersRepository.AllAsNoTracking()
            //    .FirstOrDefault(x => x.UserName == articleInputModel.UserId);
            // take the user and record its id in the article, product, conformity, etc.\\
            this.InstanceBuilder(input, entity);

            await this.experiancesRepository.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await this.experiancesRepository.All().FirstOrDefaultAsync(x => x.Id == id);
            this.experiancesRepository.Delete(entity);

            return await this.experiancesRepository.SaveChangesAsync();
        }

        private void InstanceBuilder(ExperianceCreateInputModel input, Experiance entity)
        {
            entity.Url = input.Url.Trim();
            entity.Logo = input.Logo.Trim();
            entity.Company = input.Company.Trim();
            entity.Job = input.Job;
            entity.StartDate = input.StartDate.ToUniversalTime();
            entity.IconClassName = input.IconClassName.Trim();
            entity.Details = input.Details.Trim();

            this.EndDateParser(input, entity);
        }

        private void EndDateParser(ExperianceCreateInputModel input, Experiance entity)
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
