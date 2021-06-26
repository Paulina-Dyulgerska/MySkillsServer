namespace MySkillsServer.Services.Data
{
    using System.Threading.Tasks;

    public interface IReCaptchaService
    {
        Task<bool> IsReCaptchaValid(string token);
    }
}
