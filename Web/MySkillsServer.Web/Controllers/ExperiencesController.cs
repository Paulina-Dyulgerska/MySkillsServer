namespace MySkillsServer.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Data;
    using MySkillsServer.Web.ViewModels.Experiences;

    [Route("api/[controller]")]
    [ApiController]
    public class ExperiencesController : ControllerBase
    {
        private readonly IExperiencesService experiencesService;
        private readonly UserManager<ApplicationUser> userManager;

        public ExperiencesController(IExperiencesService experiencesService, UserManager<ApplicationUser> userManager)
        {
            this.experiencesService = experiencesService;
            this.userManager = userManager;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ExperienceExportModel>>> GetAll()
        {
            var models = await this.experiencesService.GetAllAsNoTrackingOrderedAsync<ExperienceExportModel>();

            if (models == null)
            {
                return this.NotFound();
            }

            return this.Ok(models);
        }

        // GET /api/educations/id and api/educations?id=1234
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ExperienceExportModel>> GetById(int id)
        {
            var model = await this.experiencesService.GetByIdAsync<ExperienceExportModel>(id);

            if (model == null)
            {
                return this.NotFound();
            }

            return this.Ok(model);
        }

        // [Authorize]
        [HttpPost]
        [IgnoreAntiforgeryTokenAttribute]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Post(ExperienceCreateInputModel input)
        {
            // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            // var user = await this.userManager.GetUserAsync(this.User);

            // await this.educationsService.CreateAsync(input, user.Id);
            var inputId = await this.experiencesService.CreateAsync(input);
            var model = await this.experiencesService.GetByIdAsync<ExperienceExportModel>(inputId);

            return this.CreatedAtAction(nameof(this.GetById), new { id = model.Id }, model);
        }

        // [Authorize]
        [HttpPut("{id}")]
        [IgnoreAntiforgeryTokenAttribute]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ExperienceExportModel>> Put(int id, ExperienceEditInputModel input)
        {
            if (id != input.Id)
            {
                return this.BadRequest();
            }

            var model = await this.experiencesService.GetByIdAsync<ExperienceExportModel>(id);

            if (model == null)
            {
                return this.NotFound();
            }

            // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            // var user = await this.userManager.GetUserAsync(this.User);

            // await this.experiencesService.EditAsync(input, user.Id);
            await this.experiencesService.EditAsync(input);

            return this.NoContent();
        }

        // [Authorize]
        [HttpDelete("{id}")]
        [IgnoreAntiforgeryTokenAttribute]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(int id)
        {
            var model = await this.experiencesService.GetByIdAsync<ExperienceExportModel>(id);

            if (model == null)
            {
                return this.NotFound();
            }

            await this.experiencesService.DeleteAsync(id);

            return this.Ok();
        }
    }
}
