namespace MySkillsServer.Services.Data
{
    using System.Threading.Tasks;

    using MySkillsServer.Data.Models;
    using MySkillsServer.Web.ViewModels.Certificates;

    public interface ICertificatesService : IService<string>
    {
        Task<string> CreateAsync(CertificateCreateInputModel input, string userId, string certificatesFilesDirectory);

        Task<string> EditAsync(CertificateEditInputModel input, string userId, string certificatesFilesDirectory);

        Task<int> DeleteAsync(string id, string userId);
    }
}
