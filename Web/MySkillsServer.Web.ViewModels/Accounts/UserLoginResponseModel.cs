namespace MySkillsServer.Web.ViewModels.Accounts
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    // ReSharper disable once InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed. Suppression is OK here.")]
    public class UserLoginResponseModel
    {
        // TODO: Check why this approach doesn't work: [DataMember(Name = "access_token")]
        // ReSharper disable once InconsistentNaming
        // [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed. Suppression is OK here.")]
        // public string access_token { get; set; }
        public string accessToken { get; set; }

        public string userEmail { get; set; }

        public int expiresIn { get; set; }

        public IEnumerable<string> roles { get; set; }
    }
}
