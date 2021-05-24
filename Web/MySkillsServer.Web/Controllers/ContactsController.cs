namespace MySkillsServer.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Data;
    using MySkillsServer.Web.ViewModels.Contacts;

    // REST /api/contacts
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class ContactsController : ControllerBase
    {
        private readonly IContactsService contactsService;
        private readonly UserManager<ApplicationUser> userManager;

        public ContactsController(IContactsService contactsService, UserManager<ApplicationUser> userManager)
        {
            this.contactsService = contactsService;
            this.userManager = userManager;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        //public async Task<ActionResult<IEnumerable<ContactExportModel>>> GetAll()
        public async Task<IActionResult> GetAll()
        {
            var models = await this.contactsService.GetAllAsNoTrackingOrderedAsync<ContactExportModel>();

            if (models == null)
            {
                return this.NotFound();
            }

            return this.Ok(models);
        }

        // GET /api/contacts/id and api/contacts?id=1234
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ContactExportModel>> GetById(int id)
        {
            var model = await this.contactsService.GetByIdAsync<ContactExportModel>(id);

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
        public async Task<ActionResult> Post(ContactCreateInputModel input)
        {
            // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            // var user = await this.userManager.GetUserAsync(this.User);

            // await this.contactsService.CreateAsync(input, user.Id);
            var inputId = await this.contactsService.CreateAsync(input);
            var model = await this.contactsService.GetByIdAsync<ContactExportModel>(inputId);

            return this.CreatedAtAction(nameof(this.GetById), new { id = model.Id }, model);
        }

        // [Authorize]
        [HttpPut("{id}")]
        [IgnoreAntiforgeryTokenAttribute]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ContactExportModel>> Put(int id, ContactEditInputModel input)
        {
            if (id != input.Id)
            {
                return this.BadRequest();
            }

            var model = await this.contactsService.GetByIdAsync<ContactExportModel>(id);

            if (model == null)
            {
                return this.NotFound();
            }

            // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            // var user = await this.userManager.GetUserAsync(this.User);

            // await this.contactsService.EditAsync(input, user.Id);
            await this.contactsService.EditAsync(input);

            return this.NoContent();
        }

        // [Authorize]
        [HttpDelete("{id}")]
        [IgnoreAntiforgeryTokenAttribute]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(int id)
        {
            var model = await this.contactsService.GetByIdAsync<ContactExportModel>(id);

            if (model == null)
            {
                return this.NotFound();
            }

            await this.contactsService.DeleteAsync(id);

            return this.Ok();
        }
    }
}
