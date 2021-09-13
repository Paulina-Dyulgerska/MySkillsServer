namespace MySkillsServer.Services.Data
{
    using System.Threading.Tasks;

    using MySkillsServer.Web.ViewModels.FileDeliver;

    public interface IFileDeliverService
    {
        Task<FileDeliverExportModel> GetFileFromBlobStorage(string inputFileUrl);
    }
}
