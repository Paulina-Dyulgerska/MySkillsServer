namespace MySkillsServer.Services.Data
{
    using System.Threading.Tasks;

    using MySkillsServer.Data.Models;
    using MySkillsServer.Web.ViewModels.Accounts;

    public interface IAccountsService
    {
        Task<UserLoginResponseModel> Authenticate(ApplicationUser input);
    }
}
