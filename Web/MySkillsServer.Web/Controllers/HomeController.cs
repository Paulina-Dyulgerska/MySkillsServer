namespace MySkillsServer.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Data;
    using MySkillsServer.Web.ViewModels;
    using MySkillsServer.Web.ViewModels.Certificates;

    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICertificatesService certificatesService;

        public HomeController(
            UserManager<ApplicationUser> userManager,
            ICertificatesService certificatesService)
        {
            this.userManager = userManager;
            this.certificatesService = certificatesService;
        }
        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }

        public async Task<ActionResult> Post(CertificateCreateInputModel input)
        {
            var inputId = await this.certificatesService.CreateAsync(input, "8bdd4ddc-2a5d-4370-8da6-2c855cbae76a");

            var model = await this.certificatesService.GetByIdAsync<CertificateExportModel>(inputId);

            return this.View(nameof(this.Index));
        }
    }
}
