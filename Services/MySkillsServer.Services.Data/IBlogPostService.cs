namespace MySkillsServer.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MySkillsServer.Web.ViewModels.BlogPosts;
    using MySkillsServer.Web.ViewModels.Comments;

    public interface IBlogPostService : IService<string>
    {
        Task<string> CreateAsync(BlogPostCreateInputModel input, string userId, string imageFilesDirectory);

        Task<string> EditAsync(BlogPostEditInputModel input, string userId, string imageFilesDirectory);

        Task<int> EditLikesAsync(BlogPostEditLikesInputModel input);

        //Task<IEnumerable<CommentExportModel>> GetAllCommentsAsync(string id);

        Task AddCommentAsync(CommentInputModel input, string userId);

        Task<int> DeleteAsync(string id, string userId);
    }
}
