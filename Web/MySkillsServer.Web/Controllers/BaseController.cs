namespace MySkillsServer.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class BaseController : ControllerBase
    {
        //private readonly IOptions<ReCaptchaSettings> googleReCaptchaSettings;

        //public BaseController(IOptions<ReCaptchaSettings> googleReCaptchaSettings)
        //{
        //    this.googleReCaptchaSettings = googleReCaptchaSettings;
        //}

        //public bool ReCaptchaPassed(string getReCaptchaResponse)
        //{
        //    var httpClient = new HttpClient();

        //    var result = httpClient
        //        .GetAsync($"{this.googleReCaptchaSettings.Value.ApiUrl}?secret={this.googleReCaptchaSettings.Value.Secret}{getReCaptchaResponse}")
        //        .Result;

        //    if (result.StatusCode != HttpStatusCode.OK)
        //    {
        //        return false;
        //    }

        //    string jsonResult = result.Content.ReadAsStringAsync().Result;
        //    dynamic jsonData = JsonSerializer.Deserialize<dynamic>(jsonResult);

        //    if (jsonData.success != "true" || jsonData.score <= 0.5m)
        //    {
        //        return false;
        //    }

        //    return true;
        //}
    }
}
