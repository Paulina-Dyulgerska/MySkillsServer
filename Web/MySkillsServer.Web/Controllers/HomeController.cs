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
            // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            // var user = await this.userManager.GetUserAsync(this.User);
            var user = await this.userManager.FindByEmailAsync("paulina.dyulgerska@gmail.com");

            var inputId = await this.certificatesService.CreateAsync(input, user.Id);

            var model = await this.certificatesService.GetByIdAsync<CertificateExportModel>(inputId);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
