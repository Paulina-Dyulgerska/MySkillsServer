namespace MySkillsServer.Services.Data
{
    using System.Threading.Tasks;

    using MySkillsServer.Web.ViewModels.ContactFormMessages;

    public interface IContactFormMessagesService : IService<int>
    {
        Task<int> CreateAsync(ContactFormMessageCreateInputModel input, string userId);

        Task<int> EditAsync(ContactFormMessageEditInputModel input, string userId);

        Task<int> DeleteAsync(int id);
    }
}
