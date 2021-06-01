namespace MySkillsServer.Web.Infrastructure.Middlewares.Authorization
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.IdentityModel.Tokens;

    //// JWT Authentication services 2
    public class TokenProviderOptions
    {
        public string Path { get; set; } = "/token";

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public TimeSpan Expiration { get; set; } = TimeSpan.FromDays(7);

        public Func<Task<string>> NonceGenerator { get; set; } = () => Task.FromResult(Guid.NewGuid().ToString());

        public SigningCredentials SigningCredentials { get; set; }
    }
}
