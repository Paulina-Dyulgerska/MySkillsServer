namespace MySkillsServer.Web.Controllers
{
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
            var models = await this.educationsService.GetAllAsNoTrackingOrderedAsync<EducationExportModel>();

            if (models == null)
            {
                return this.NoContent();
            }

            return this.Ok(models);
        }

        // GET /api/educations/id and api/educations?id=1234
        [HttpGet("{id}")]

        // [ProducesResponseType(200)]
        // [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var model = await this.educationsService.GetByIdAsync<EducationExportModel>(id);

            if (model == null)
            {
                return this.NotFound();
            }

            return this.Ok(model);
        }

        // [Authorize]
        [HttpPost]
        [IgnoreAntiforgeryTokenAttribute]
        public async Task<IActionResult> Post(EducationCreateInputModel input)
        {
            // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            // var user = await this.userManager.GetUserAsync(this.User);

            // await this.educationsService.CreateAsync(input, user.Id);
            var modelId = await this.educationsService.CreateAsync(input);
            var model = await this.educationsService.GetByIdAsync<EducationExportModel>(modelId);

            // return this.CreatedAtAction(nameof(this.GetById), new { id = modelId }, model);
            return this.Ok(model);
        }

        // [Authorize]
        [HttpPut("{id}")]
        [IgnoreAntiforgeryTokenAttribute]
        public async Task<IActionResult> Put(int id, EducationEditInputModel input)
        {

            if (id != input.Id)
            {
                return this.BadRequest();
            }

            var model = await this.educationsService.GetByIdAsync<EducationExportModel>(id);

            if (model == null)
            {
                return this.NotFound();
            }

            // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            // var user = await this.userManager.GetUserAsync(this.User);

            // await this.educationsService.EditAsync(input, user.Id);
            await this.educationsService.EditAsync(input);

            model = await this.educationsService.GetByIdAsync<EducationExportModel>(id);

            return this.Ok(model);
        }

        // [Authorize]
        [HttpDelete("{id}")]
        [IgnoreAntiforgeryTokenAttribute]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await this.educationsService.GetByIdAsync<EducationExportModel>(id);

            if (model == null)
            {
                return this.NotFound();
            }

            await this.educationsService.DeleteAsync(id);

            return this.Ok();
        }
    }
}
