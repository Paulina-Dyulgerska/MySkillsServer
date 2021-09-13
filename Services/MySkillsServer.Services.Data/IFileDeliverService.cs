namespace MySkillsServer.Services.Data
{
    using System.IO;
    using System.Threading.Tasks;

    using MySkillsServer.Web.ViewModels.FileDeliver;

    public interface IFileDeliverService
    {
        Task<FileDeliverExportModel> GetFileFromBlobStorage(string inputFileUrl);

        Task<Stream> GetFileFromBlobStorageForDownload(string inputFileUrl);
    }
}
