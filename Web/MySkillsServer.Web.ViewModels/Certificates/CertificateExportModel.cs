namespace MySkillsServer.Web.ViewModels.Certificates
{
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Mapping;

    public class CertificateExportModel : IMapFrom<Certificate>
    {
        public string Id { get; set; }
    }
}
