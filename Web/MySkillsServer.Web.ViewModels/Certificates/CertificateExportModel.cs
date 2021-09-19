namespace MySkillsServer.Web.ViewModels.Certificates
{
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Mapping;

    public class CertificateExportModel : IMapFrom<Certificate>
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public string FileExtension { get; set; }

        public string RemoteFileUrl { get; set; }
    }
}
