﻿namespace MySkillsServer.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Data;
    using MySkillsServer.Web.ViewModels.Experiances;

    [Route("api/[controller]")]
    [ApiController]
    public class ExperiancesController : ControllerBase
    {
        private readonly IExperiancesService experiancesService;
        private readonly UserManager<ApplicationUser> userManager;

        public ExperiancesController(IExperiancesService experiancesService, UserManager<ApplicationUser> userManager)
        {
            this.experiancesService = experiancesService;
            this.userManager = userManager;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ExperianceExportModel>>> GetAll()
        {
            var models = await this.experiancesService.GetAllAsNoTrackingOrderedAsync<ExperianceExportModel>();

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
        public async Task<ActionResult<ExperianceExportModel>> GetById(int id)
        {
            var model = await this.experiancesService.GetByIdAsync<ExperianceExportModel>(id);

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
        public async Task<ActionResult> Post(ExperianceCreateInputModel input)
        {
            // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            // var user = await this.userManager.GetUserAsync(this.User);

            // await this.educationsService.CreateAsync(input, user.Id);
            var inputId = await this.experiancesService.CreateAsync(input);
            var model = await this.experiancesService.GetByIdAsync<ExperianceExportModel>(inputId);

            return this.CreatedAtAction(nameof(this.GetById), new { id = model.Id }, model);
        }

        // [Authorize]
        [HttpPut("{id}")]
        [IgnoreAntiforgeryTokenAttribute]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ExperianceExportModel>> Put(int id, ExperianceEditInputModel input)
        {
            if (id != input.Id)
            {
                return this.BadRequest();
            }

            var model = await this.experiancesService.GetByIdAsync<ExperianceExportModel>(id);

            if (model == null)
            {
                return this.NotFound();
            }

            // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            // var user = await this.userManager.GetUserAsync(this.User);

            // await this.experiancesService.EditAsync(input, user.Id);
            await this.experiancesService.EditAsync(input);

            return this.NoContent();
        }

        // [Authorize]
        [HttpDelete("{id}")]
        [IgnoreAntiforgeryTokenAttribute]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(int id)
        {
            var model = await this.experiancesService.GetByIdAsync<ExperianceExportModel>(id);

            if (model == null)
            {
                return this.NotFound();
            }

            await this.experiancesService.DeleteAsync(id);

            return this.Ok();
        }
    }
}
