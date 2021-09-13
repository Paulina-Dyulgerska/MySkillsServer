namespace MySkillsServer.Services.Data
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Azure.Storage.Blobs;
    using MySkillsServer.Common;
    using MySkillsServer.Web.ViewModels.FileDeliver;

    public class FileDeliverService : IFileDeliverService
    {
        private readonly BlobServiceClient blobServiceClient;

        public FileDeliverService(BlobServiceClient blobServiceClient)
        {
            this.blobServiceClient = blobServiceClient;
        }

        public async Task<FileDeliverExportModel> GetFileFromBlobStorage(string inputFileUrl)
        {
            var container = this.blobServiceClient.GetBlobContainerClient(GlobalConstants
                                                                                .AzureStorageBlobContainerName);
            var fileBlob = container.GetBlobClient(inputFileUrl.Split('/').LastOrDefault());
            var downloadedFile = await fileBlob.DownloadContentAsync();

            var requestedFile = new FileDeliverExportModel
            {
                FilePath = fileBlob.Uri.AbsoluteUri,
                FileBytes = downloadedFile.Value.Content.ToArray(),
                FileContentType = downloadedFile.Value.Details.ContentType,
            };

            return requestedFile;
        }

        public async Task<Stream> GetFileFromBlobStorageForDownload(string inputFileUrl)
        {
            var container = this.blobServiceClient.GetBlobContainerClient(GlobalConstants
                                                                    .AzureStorageBlobContainerName);
            var fileBlob = container.GetBlobClient(inputFileUrl.Split('/').LastOrDefault());

            var stream = await fileBlob.OpenReadAsync();
            return stream;
        }
    }
}
