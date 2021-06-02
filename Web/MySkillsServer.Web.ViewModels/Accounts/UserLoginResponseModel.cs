namespace MySkillsServer.Web.ViewModels.Accounts
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public class UserLoginResponseModel
    {
        // TODO: Check why this approach doesn't work: [DataMember(Name = "access_token")]
        // ReSharper disable once InconsistentNaming
        // [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed. Suppression is OK here.")]
        // public string access_token { get; set; }
        public string AccessToken { get; set; }

        public int ExpiresIn { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
