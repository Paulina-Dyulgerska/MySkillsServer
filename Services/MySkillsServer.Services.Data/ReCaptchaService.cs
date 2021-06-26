namespace MySkillsServer.Services.Data
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Options;
    using MySkillsServer.Web.Infrastructure.ReCaptcha;
    using MySkillsServer.Web.Infrastructure.Settings;

    public class ReCaptchaService : IReCaptchaService
    {
        private readonly IOptions<ReCaptchaSettings> googleReCaptchaSettings;

        public ReCaptchaService(IOptions<ReCaptchaSettings> googleReCaptchaSettings)
        {
            this.googleReCaptchaSettings = googleReCaptchaSettings;
        }

        public async Task<bool> IsReCaptchaValid(string token)
        {
            var result = false;

            try
            {
                using var client = new HttpClient();

                var response = await client
                    .PostAsync(
                    $"{this.googleReCaptchaSettings.Value.ApiUrl}?secret={this.googleReCaptchaSettings.Value.Secret}&response={token}", null);
                var jsonResponse = response.Content.ReadAsStringAsync().Result;
                var captchaVerfication = JsonSerializer.Deserialize<ReCaptchaVerificationResponse>(jsonResponse);

                if (captchaVerfication.Success && captchaVerfication.Score > 0.5)
                {
                }

                result = captchaVerfication.Success;
            }
            catch (Exception e)
            {
                // TODO: fail gracefully, but log
                throw new Exception("Failed to process captcha validation", e);
            }

            return result;
        }
    }
}
