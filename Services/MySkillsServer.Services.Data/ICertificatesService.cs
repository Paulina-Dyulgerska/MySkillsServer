namespace MySkillsServer.Services.Data
{
    using System.Threading.Tasks;

    using MySkillsServer.Web.ViewModels.Certificates;

    public interface ICertificatesService : IService<int>
    {
        Task<int> CreateAsync(CertificateCreateInputModel input, string userId);

        Task<int> EditAsync(CertificateEditInputModel input, string userId);

        Task<int> DeleteAsync(int id, string userId);
    }
}
