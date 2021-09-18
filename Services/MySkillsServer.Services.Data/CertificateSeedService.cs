namespace MySkillsServer.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using MySkillsServer.Common;
    using MySkillsServer.Data.Common.Repositories;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Data.Models;

    public class CertificateSeedService : ICertificatesSeedService
    {
        private readonly IRepository<Certificate> certificatesRepository;
        private readonly IRepository<ApplicationUser> users;
        private readonly IRepository<ApplicationRole> roles;

        public CertificateSeedService(
            IRepository<Certificate> certificatesRepository,
            IRepository<ApplicationUser> users,
            IRepository<ApplicationRole> roles)
        {
            this.certificatesRepository = certificatesRepository;
            this.users = users;
            this.roles = roles;
        }

        public async Task CreateAsync(CertificateDTO certificateDTO)
        {
            var adminRoleId = this.roles.AllAsNoTracking().FirstOrDefault(x => x.Name == GlobalConstants.AdministratorRoleName).Id;
            var user = this.users.All().FirstOrDefault(x => x.Roles.Any(x => x.RoleId == adminRoleId));

            var certificate = new Certificate
            {
                FileName = certificateDTO.FileName,
                FileExtension = certificateDTO.FileExtension,
                ImageFileExtension = certificateDTO.ImageFileExtension,
                RemoteFileUrl = certificateDTO.RemoteFileUrl,
                ImageRemoteFileUrl = certificateDTO.ImageRemoteFileUrl,
                User = user,
            };

            await this.certificatesRepository.AddAsync(certificate);

            await this.certificatesRepository.SaveChangesAsync();
        }
    }
}
