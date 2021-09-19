namespace MySkillsServer.Web.ViewModels.Certificates
{
    using Microsoft.AspNetCore.Http;

    public class CertificateEditInputModel
    {
        public int Id { get; set; }

        public IFormFile InputFile { get; set; }
    }
}
