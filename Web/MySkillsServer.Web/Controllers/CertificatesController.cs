namespace MySkillsServer.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MySkillsServer.Common;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Data;
    using MySkillsServer.Web.ViewModels.Certificates;

    [Route("api/[controller]")]
    [ApiController]
    public class CertificatesController : BaseController
    {
        private const string CertificatesDirName = "certificates";
        private readonly ICertificatesService certificatesService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment environment;
        private readonly string certificatesFilesDirectory;

        public CertificatesController(
            ICertificatesService certificatesService,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment environment)
        {
            this.certificatesService = certificatesService;
            this.userManager = userManager;
            this.environment = environment;
            this.certificatesFilesDirectory = $"{this.environment.WebRootPath}/{CertificatesDirName}";
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAll()
        {
            var models = await this.certificatesService
                .GetAllAsNoTrackingOrderedAsync<CertificateExportModel>();

            if (models == null)
            {
                return this.NotFound();
            }

            return this.Ok(models);
        }

        // GET /api/certificates/id and api/certificates?id=1234
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CertificateExportModel>> GetById(string id)
        {
            var model = await this.certificatesService.GetByIdAsync<CertificateExportModel>(id);

            if (model == null)
            {
                return this.NotFound();
            }

            return this.Ok(model);
        }

        // [Authorize]
        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [IgnoreAntiforgeryTokenAttribute]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Post(CertificateCreateInputModel input)
        {
            // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await this.userManager.GetUserAsync(this.User);

            var inputId = await this.certificatesService.CreateAsync(input, user.Id, this.certificatesFilesDirectory);

            var model = await this.certificatesService.GetByIdAsync<CertificateExportModel>(inputId);

            return this.CreatedAtAction(nameof(this.GetById), new { id = model.Id }, model);
        }

        // [Authorize]
        [HttpPut("{id}")]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [IgnoreAntiforgeryTokenAttribute]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CertificateExportModel>> Put(string id, [FromForm] CertificateEditInputModel input)
        {
            if (id != input.Id)
            {
                return this.BadRequest();
            }

            var model = await this.certificatesService.GetByIdAsync<CertificateExportModel>(id);

            if (model == null)
            {
                return this.NotFound();
            }

            // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await this.userManager.GetUserAsync(this.User);

            await this.certificatesService.EditAsync(input, user.Id, this.certificatesFilesDirectory);

            return this.NoContent();
        }

        // [Authorize]
        [HttpDelete("{id}")]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [IgnoreAntiforgeryTokenAttribute]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(string id)
        {
            var model = await this.certificatesService.GetByIdAsync<CertificateExportModel>(id);

            if (model == null)
            {
                return this.NotFound();
            }

            var user = await this.userManager.GetUserAsync(this.User);

            await this.certificatesService.DeleteAsync(id, user.Id);

            return this.Ok();
        }
    }
}
