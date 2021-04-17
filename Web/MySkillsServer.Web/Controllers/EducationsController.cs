﻿namespace MySkillsServer.Web.Controllers
{
    using System.Collections.Generic;
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
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<EducationExportModel>>> GetAll()
        {
            var models = await this.educationsService.GetAllAsNoTrackingOrderedAsync<EducationExportModel>();

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
        public async Task<ActionResult<EducationExportModel>> GetById(int id)
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
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Post(EducationCreateInputModel input)
        {
            // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            // var user = await this.userManager.GetUserAsync(this.User);

            // await this.educationsService.CreateAsync(input, user.Id);
            var inputId = await this.educationsService.CreateAsync(input);
            input.Id = inputId;

            return this.CreatedAtAction(nameof(this.GetById), new { id = input.Id }, input);
        }

        // [Authorize]
        [HttpPut("{id}")]
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

        // [Authorize]
        [HttpDelete("{id}")]
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
