namespace MySkillsServer.Web.Controllers
{
    using System.Collections.Generic;
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
        public async Task<IEnumerable<EducationExportModel>> GetAll()
        {
            var models = await this.educationsService.GetAllAsNoTrackingOrderedAsync<EducationExportModel>();

            return models;
        }

        // GET /api/educations/id and api/educations?id=1234
        [HttpGet("{id}")]

        // [ProducesResponseType(200)]
        // [ProducesResponseType(404)]
        public async Task<ActionResult<EducationExportModel>> GetById(int id)
        {
            var model = await this.educationsService.GetByIdAsync<EducationExportModel>(id);

            if (model == null)
            {
                return this.NotFound();
            }

            return model;
        }

        [HttpPost]
        [IgnoreAntiforgeryTokenAttribute]

        // [Authorize]
        public async Task<ActionResult<EducationCreateInputModel>> Create(EducationCreateInputModel input)
        {
            // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            // var user = await this.userManager.GetUserAsync(this.User);

            // await this.educationsService.CreateAsync(input, user.Id);
            await this.educationsService.CreateAsync(input);

            return input;
        }

        [HttpPut("{id}")]
        [IgnoreAntiforgeryTokenAttribute]

        // [Authorize]
        public async Task<ActionResult<EducationEditInputModel>> Edit(int id, EducationEditInputModel input)
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
        [IgnoreAntiforgeryTokenAttribute]

        // [Authorize]
        public async Task<ActionResult<int>> Delete(int id)
        {
            var model = await this.educationsService.GetByIdAsync<EducationExportModel>(id);

            if (model == null)
            {
                return this.NotFound();
            }

            return await this.educationsService.DeleteAsync(id);
        }
    }
}
