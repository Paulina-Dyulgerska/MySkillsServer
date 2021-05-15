namespace MySkillsServer.Services.Data.Models
{
    using MySkillsServer.Data.Models;
    using MySkillsServer.Services.Mapping;

    public class AdminUserDTO : IMapTo<ApplicationUser>
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool EmailConfirmed { get; set; }
    }
}
