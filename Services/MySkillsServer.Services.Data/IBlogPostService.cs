namespace MySkillsServer.Services.Data
{
    using System.Threading.Tasks;

    using MySkillsServer.Web.ViewModels.BlogPosts;

    public interface IBlogPostService : IService<string>
    {
        Task<string> CreateAsync(BlogPostCreateInputModel input, string userId, string imageFilesDirectory);

        Task<string> EditAsync(BlogPostEditInputModel input, string userId, string imageFilesDirectory);

        Task<int> EditLikesAsync(BlogPostEditLikesInputModel input);

        Task<int> DeleteAsync(string id, string userId);
    }
}
