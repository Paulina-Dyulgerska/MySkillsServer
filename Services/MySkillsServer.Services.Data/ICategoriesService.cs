namespace MySkillsServer.Services.Data
{
    using System.Threading.Tasks;

    using MySkillsServer.Web.ViewModels.Categories;

    public interface ICategoriesService : IService<int>
    {
        Task<int> CreateAsync(CategoryCreateInputModel input, string userId);

        Task<int> EditAsync(CategoryEditInputModel input, string userId);

        Task<int> DeleteAsync(int id, string userId);
    }
}
