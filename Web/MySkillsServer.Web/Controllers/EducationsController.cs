namespace MySkillsServer.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Data;
    using MySkillsServer.Web.ViewModels.Educations;

    // REST /api/educations
    [Route("api/[controller]")]
    [ApiController]
    public class EducationsController : ControllerBase
    {
        private readonly IEducationsService educationsService;
        private readonly UserManager<ApplicationUser> userManager;

        public EducationsController(IEducationsService educationsService, UserManager<ApplicationUser> userManager)
        {
            this.educationsService = educationsService;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var model = await this.educationsService.GetAllAsNoTrackingOrderedAsync<EducationExportModel>();

            return (IActionResult)model;
        }

        // GET /api/educations/id and api/educations?id=1234
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var model = await this.educationsService.GetByIdAsync<EducationExportModel>(id);
            if (model == null)
            {
                return this.NotFound();
            }

            return (IActionResult)model;
        }

        [HttpPost]
        // [Authorize]
        public async Task<IActionResult> Create(EducationEditInputModel input)
        {
            // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await this.userManager.GetUserAsync(this.User);

            await this.educationsService.CreateAsync(input, user.Id);

            return (IActionResult)input;
        }

        [HttpPatch]
        // [Authorize]
        public async Task<IActionResult> Edit(EducationEditInputModel input)
        {
            // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await this.userManager.GetUserAsync(this.User);

            await this.educationsService.CreateAsync(input, user.Id);

            return (IActionResult)input;
        }

        [HttpDelete("{id}")]
        // [Authorize]
        public int Delete(int id)
        {
            this.educationsService.DeleteAsync(id);

            return id;
        }
    }
}
