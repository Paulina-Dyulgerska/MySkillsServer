namespace MySkillsServer.Web.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using Azure.Storage.Blobs;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using MySkillsServer.Services.Data;

    [ApiController]
    [Route("/api/[controller]")]
    public class FileDeliverController : ControllerBase
    {
        private readonly IFileDeliverService fileDeliverService;
        private readonly ILogger<FileDeliverController> logger;
        private readonly BlobServiceClient blobServiceClient;

        public FileDeliverController(
            IFileDeliverService fileDeliverService,
            ILogger<FileDeliverController> logger,
            BlobServiceClient blobServiceClient)
        {
            this.fileDeliverService = fileDeliverService;
            this.logger = logger;
            this.blobServiceClient = blobServiceClient;
        }

        // Get /api/fileDeliver/show/id
        [HttpGet("show/{id}")]
        public async Task<IActionResult> ShowModalDocument(string id)
        {
            var inline = true;
            try
            {
                return await this.GetFile(id, inline);
            }
            catch (Exception ex)
            {
                this.logger.LogError(
                    1978,
                    $"RequestID: {Activity.Current?.Id ?? this.HttpContext.TraceIdentifier}; Api Error: {ex}");

                throw;
            }
        }

        // Get /api/fileDeliver/download/id
        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadModalDocument(string id)
        {
            var inLine = false;

            try
            {
                return await this.GetFile(id, inLine);
            }
            catch (Exception ex)
            {
                this.logger.LogError(
                    1978,
                    $"RequestID: {Activity.Current?.Id ?? this.HttpContext.TraceIdentifier}; Api Error: {ex}");

                throw;
            }
        }

        private async Task<IActionResult> GetFile(string id, bool inLine)
        {
            var contentDisposition = new System.Net.Mime.ContentDisposition
            {
                DispositionType = "attachment",
                FileName = id.Split('/').LastOrDefault(),
                Inline = inLine,
            };

            this.Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

            var fileSavedInRemoteStorageContainer = id.StartsWith("http");

            var fileData = await this.fileDeliverService.GetFileFromBlobStorage(id);

            this.logger.LogInformation($"API {nameof(this.DownloadModalDocument)} from remote storage success.");

            return this.File(fileData.FileBytes, System.Net.Mime.MediaTypeNames.Application.Pdf);
        }
    }
}
