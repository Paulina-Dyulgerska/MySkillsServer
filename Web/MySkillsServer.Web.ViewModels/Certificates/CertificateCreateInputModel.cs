namespace MySkillsServer.Web.ViewModels.Certificates
{
    using Microsoft.AspNetCore.Http;

    public class CertificateCreateInputModel
    {
        public IFormFile InputFile { get; set; }
    }
}
