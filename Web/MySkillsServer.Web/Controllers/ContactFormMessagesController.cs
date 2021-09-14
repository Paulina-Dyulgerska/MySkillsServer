namespace MySkillsServer.Web.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using MySkillsServer.Common;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Data;
    using MySkillsServer.Services.Messaging;
    using MySkillsServer.Web.ViewModels.ContactFormMessages;

    // REST /api/contactFormMessages
    [Route("api/[controller]")]
    [ApiController]
    public class ContactFormMessagesController : ControllerBase
    {
        private readonly IContactFormMessagesService contactFormMessagesService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailSender emailSender;
        private readonly ILogger<ContactFormMessagesController> logger;

        public ContactFormMessagesController(
            IContactFormMessagesService contactFormMessagesService,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            ILogger<ContactFormMessagesController> logger)
        {
            this.contactFormMessagesService = contactFormMessagesService;
            this.userManager = userManager;
            this.emailSender = emailSender;
            this.logger = logger;
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

        // TODO - not from Form but FromBody!!!!!
        [HttpPost]
        [IgnoreAntiforgeryTokenAttribute]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Post([FromForm] ContactFormMessageCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                // TODO - return model state errrors!!!!
                return this.BadRequest();
            }

            ContactFormMessageExportModel model;

            try
            {
                // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = await this.userManager.GetUserAsync(this.User);

                // TODO: Extract to IP provider (service)
                var ip = this.HttpContext.Connection.RemoteIpAddress.ToString();
                input.Ip = ip;

                var inputId = await this.contactFormMessagesService.CreateAsync(input, user?.Id);
                model = await this.contactFormMessagesService.GetByIdAsync<ContactFormMessageExportModel>(inputId);

                // send email to site admin
                await this.emailSender.SendEmailAsync(
                    GlobalConstants.SystemEmail,
                    GlobalConstants.SystemName,
                    GlobalConstants.SystemEmail,
                    GlobalConstants.SystemName,
                    $"You have message from {input.Name}",
                    $"Message content: {input.Subject} - {input.Message}");

                // send email to contact message sender
                await this.emailSender.SendEmailAsync(
                    GlobalConstants.SystemEmail,
                    GlobalConstants.SystemName,
                    input.Email,
                    input.Name,
                    "Thank you for your message",
                    $"Dear {input.Name},\r\n Thank you for your interest on our site and the message sent!\r\n We will contact you as soon as we review your request.\r\n\r\nKind Regards,\r\nMySkills Team");

                this.logger.LogInformation($"Send contact message to user and admin, record message in the DB.");
            }
            catch (Exception ex)
            {
                this.logger.LogError($"RequestID: {Activity.Current?.Id ?? this.HttpContext.TraceIdentifier}; Contact message creation failed: {ex}.");

                // TODO - return model state errrors!!!!
                return this.BadRequest();
            }

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
