namespace MySkillsServer.Web.Infrastructure.Settings
{
    public class ReCaptchaSettings
    {
        public const string ReCaptcha = "GoogleReCaptchaV3Settings";

        public string SiteKey { get; set; }

        public string Secret { get; set; }

        public string ApiUrl { get; set; }
    }
}
