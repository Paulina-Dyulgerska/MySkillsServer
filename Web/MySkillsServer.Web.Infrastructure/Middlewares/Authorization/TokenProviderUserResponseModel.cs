namespace MySkillsServer.Web.Infrastructure.Middlewares.Authorization
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    // ReSharper disable once InconsistentNaming
    // [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed. Suppression is OK here.")]
    public class TokenProviderUserResponseModel
    {
        // ReSharper disable once InconsistentNaming
        // [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed. Suppression is OK here.")]
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }

        [JsonPropertyName("userEmail")]
        public string UserEmail { get; set; }

        [JsonPropertyName("expiresIn")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("roles")]
        public IEnumerable<string> Roles { get; set; }
    }
}
