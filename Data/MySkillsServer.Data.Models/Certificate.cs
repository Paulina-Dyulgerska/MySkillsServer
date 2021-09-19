namespace MySkillsServer.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using MySkillsServer.Data.Common.Models;

    public class Certificate : BaseModel<int>
    {
        public string FileName { get; set; }

        public string FileExtension { get; set; }

        public string RemoteFileUrl { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
