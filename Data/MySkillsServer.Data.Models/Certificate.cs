namespace MySkillsServer.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using MySkillsServer.Data.Common.Models;

    public class Certificate : BaseModel<string>
    {
        public Certificate()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string FileName { get; set; }

        public string FileExtension { get; set; }

        public string ImageFileExtension { get; set; }

        public string RemoteFileUrl { get; set; }

        public string ImageRemoteFileUrl { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
