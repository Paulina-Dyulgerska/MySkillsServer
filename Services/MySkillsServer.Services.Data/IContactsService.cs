namespace MySkillsServer.Services.Data
{
    using System.Threading.Tasks;

    using MySkillsServer.Web.ViewModels.Contacts;

    public interface IContactsService : IService<int>
    {
        Task<int> CreateAsync(ContactCreateInputModel input, string userId);

        Task<int> EditAsync(ContactEditInputModel input, string userId);

        Task<int> DeleteAsync(int id, string userId);
    }
}
