namespace MySkillsServer.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MySkillsServer.Common;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Data;
    using MySkillsServer.Web.ViewModels.ContactFormMessages;

    // REST /api/contactFormMessages
    [Route("api/[controller]")]
    [ApiController]
    public class ContactFormMessagesController : ControllerBase
    {
        private readonly IContactFormMessagesService contactFormMessagesService;
        private readonly UserManager<ApplicationUser> userManager;

        public ContactFormMessagesController(
            IContactFormMessagesService contactFormMessagesService,
            UserManager<ApplicationUser> userManager)
        {
            this.contactFormMessagesService = contactFormMessagesService;
            this.userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAll()
        {
            var models = await this.contactFormMessagesService
                .GetAllAsNoTrackingOrderedAsync<ContactFormMessageExportModel>();

            if (models == null)
            {
                return this.NotFound();
            }

            return this.Ok(models);
        }

        // GET /api/contactFormMessages/id and api/contactFormMessages?id=1234
        [HttpGet("{id}")]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ContactFormMessageExportModel>> GetById(int id)
        {
            var model = await this.contactFormMessagesService.GetByIdAsync<ContactFormMessageExportModel>(id);

            if (model == null)
            {
                return this.NotFound();
            }

            return this.Ok(model);
        }

        [HttpPost]
        [IgnoreAntiforgeryTokenAttribute]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        // TODO - not from Form but FromBody!!!!!
        public async Task<ActionResult> Post([FromForm] ContactFormMessageCreateInputModel input)
        {
            // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await this.userManager.GetUserAsync(this.User);

            var inputId = await this.contactFormMessagesService.CreateAsync(input, user.Id);
            var model = await this.contactFormMessagesService.GetByIdAsync<ContactFormMessageExportModel>(inputId);

            return this.CreatedAtAction(nameof(this.GetById), new { id = model.Id }, model);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [IgnoreAntiforgeryTokenAttribute]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ContactFormMessageExportModel>> Put(
            int id,
            ContactFormMessageEditInputModel input)
        {
            if (id != input.Id)
            {
                return this.BadRequest();
            }

            var model = await this.contactFormMessagesService.GetByIdAsync<ContactFormMessageExportModel>(id);

            if (model == null)
            {
                return this.NotFound();
            }

            // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await this.userManager.GetUserAsync(this.User);

            await this.contactFormMessagesService.EditAsync(input, user.Id);

            return this.NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [IgnoreAntiforgeryTokenAttribute]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(int id)
        {
            var model = await this.contactFormMessagesService.GetByIdAsync<ContactFormMessageExportModel>(id);

            if (model == null)
            {
                return this.NotFound();
            }

            await this.contactFormMessagesService.DeleteAsync(id);

            return this.Ok();
        }
    }
}
