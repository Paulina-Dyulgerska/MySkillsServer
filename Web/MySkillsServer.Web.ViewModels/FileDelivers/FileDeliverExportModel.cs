namespace MySkillsServer.Web.ViewModels.FileDeliver
{
    public class FileDeliverExportModel
    {
        public string FilePath { get; set; }

        public string FileContentType { get; set; }

        public byte[] FileBytes { get; set; }
    }
}
