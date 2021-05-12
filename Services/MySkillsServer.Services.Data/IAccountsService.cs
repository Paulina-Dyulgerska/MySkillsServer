namespace MySkillsServer.Services.Data
{
    using MySkillsServer.Data.Models;
    using MySkillsServer.Web.ViewModels.Accounts;

    public interface IAccountsService
    {
        UserLoginResponseModel Authenticate(ApplicationUser user);
    }
}
