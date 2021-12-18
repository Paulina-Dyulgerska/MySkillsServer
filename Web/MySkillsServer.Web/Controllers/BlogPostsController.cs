namespace MySkillsServer.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MySkillsServer.Common;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Data;
    using MySkillsServer.Web.ViewModels.BlogPosts;
    using MySkillsServer.Web.ViewModels.Comments;

    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private const string ImageDirName = "images";
        private readonly IBlogPostService blogPostService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment environment;
        private readonly string imageFilesDirectory;

        public BlogPostsController(
            IBlogPostService blogPostService,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment environment)
        {
            this.blogPostService = blogPostService;
            this.userManager = userManager;
            this.environment = environment;
            this.imageFilesDirectory = $"{this.environment.WebRootPath}/{ImageDirName}";
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAll()
        {
            var models = await this.blogPostService
                .GetAllAsNoTrackingOrderedAsync<BlogPostExportModel>();

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
        public async Task<ActionResult<BlogPostExportModel>> GetById(string id)
        {
            var model = await this.blogPostService.GetByIdAsync<BlogPostExportModel>(id);

            if (model == null)
            {
                return this.NotFound();
            }

            return this.Ok(model);
        }

        // [Authorize]
        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [IgnoreAntiforgeryTokenAttribute]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Post(BlogPostCreateInputModel input)
        {
            // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await this.userManager.GetUserAsync(this.User);

            var inputId = await this.blogPostService.CreateAsync(input, user.Id, this.imageFilesDirectory);

            var model = await this.blogPostService.GetByIdAsync<BlogPostExportModel>(inputId);

            return this.CreatedAtAction(nameof(this.GetById), new { id = model.Id }, model);
        }

        // [Authorize]
        [HttpPut("{id}")]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [IgnoreAntiforgeryTokenAttribute]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<BlogPostExportModel>> Put(string id, [FromForm] BlogPostEditInputModel input)
        {
            if (id != input.Id)
            {
                return this.BadRequest();
            }

            var model = await this.blogPostService.GetByIdAsync<BlogPostExportModel>(id);

            if (model == null)
            {
                return this.NotFound();
            }

            // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await this.userManager.GetUserAsync(this.User);

            await this.blogPostService.EditAsync(input, user.Id, this.imageFilesDirectory);

            return this.NoContent();
        }

        // [Authorize]
        [HttpPatch("likes/{id}")]
        [IgnoreAntiforgeryTokenAttribute]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<BlogPostExportModel>> Patch([FromForm] BlogPostEditLikesInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            var result = await this.blogPostService.EditLikesAsync(input);

            return this.Ok(result);
        }

        //// [Authorize]
        //[HttpGet("{id}/comments")]
        //[IgnoreAntiforgeryTokenAttribute]
        //[ProducesResponseType(200)]
        //[ProducesResponseType(404)]
        //public async Task<ActionResult<BlogPostExportModel>> Get(string id)
        //{
        //    if (!this.ModelState.IsValid)
        //    {
        //        return this.BadRequest();
        //    }

        //    // var user = await this.userManager.GetUserAsync(this.User);
        //    var result = await this.blogPostService.GetAllCommentsAsync(id);

        //    return this.Ok(result);
        //}

        // [Authorize]
        [HttpPost("comments/add/{id}")]
        [IgnoreAntiforgeryTokenAttribute]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<BlogPostExportModel>> Post([FromForm] CommentInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            var user = await this.userManager.GetUserAsync(this.User);

            var result = await this.blogPostService.AddCommentAsync(input, user.Id);

            return this.Ok(result);
        }

        // [Authorize]
        [HttpDelete("{id}")]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [IgnoreAntiforgeryTokenAttribute]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(string id)
        {
            var model = await this.blogPostService.GetByIdAsync<BlogPostExportModel>(id);

            if (model == null)
            {
                return this.NotFound();
            }

            var user = await this.userManager.GetUserAsync(this.User);

            await this.blogPostService.DeleteAsync(id, user.Id);

            return this.Ok();
        }
    }
}
