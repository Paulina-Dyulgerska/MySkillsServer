namespace MySkillsServer.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MySkillsServer.Common;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Data;
    using MySkillsServer.Web.ViewModels.Educations;

    // REST /api/educations
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    public class EducationsController : ControllerBase
    {
        private readonly IEducationsService educationsService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public EducationsController(IEducationsService educationsService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.educationsService = educationsService;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAll()
        {
            var models = await this.educationsService.GetAllAsNoTrackingOrderedAsync<EducationExportModel>();

            if (models == null)
            {
                return this.NotFound();
            }

            var user = this.User;
            var asdd = user.Identity.IsAuthenticated;

            await this.signInManager.SignOutAsync();

            return this.Ok(models);
        }

        // GET /api/educations/id and api/educations?id=1234
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<EducationExportModel>> GetById(int id)
        {
            var model = await this.educationsService.GetByIdAsync<EducationExportModel>(id);

            if (model == null)
            {
                return this.NotFound();
            }

            return this.Ok(model);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [IgnoreAntiforgeryTokenAttribute]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Post(EducationCreateInputModel input)
        {
            // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            // var user = await this.userManager.GetUserAsync(this.User);

            // await this.educationsService.CreateAsync(input, user.Id);
            var inputId = await this.educationsService.CreateAsync(input);
            var model = await this.educationsService.GetByIdAsync<EducationExportModel>(inputId);

            return this.CreatedAtAction(nameof(this.GetById), new { id = model.Id }, model);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [IgnoreAntiforgeryTokenAttribute]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<EducationExportModel>> Put(int id, EducationEditInputModel input)
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

            return this.NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [IgnoreAntiforgeryTokenAttribute]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(int id)
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
