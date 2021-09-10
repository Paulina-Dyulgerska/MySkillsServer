namespace MySkillsServer.Services.Data
{
    using System.Threading.Tasks;

    using MySkillsServer.Web.ViewModels.Educations;

    public interface IEducationsService : IService<int>
    {
        Task<int> CreateAsync(EducationCreateInputModel input, string userId);

        Task<int> EditAsync(EducationEditInputModel input, string userId);

        Task<int> DeleteAsync(int id, string userId);
    }
}
