namespace MySkillsServer.Web.Infrastructure.ValidationAttributes
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Net;
    using System.Net.Http;
    using System.Text.Json;

    using Microsoft.Extensions.Configuration;
    using MySkillsServer.Web.Infrastructure.ReCaptcha;

    public class GoogleReCaptchaValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult(
                    "Google reCAPTCHA validation failed. Value is null or empty.",
                    new[] { validationContext.MemberName });
            }

            var configuration = (IConfiguration)validationContext.GetService(typeof(IConfiguration));
            if (configuration == null || string.IsNullOrWhiteSpace(configuration["GoogleReCaptchaV3Settings:Secret"]))
            {
                return new ValidationResult(
                    "Google reCAPTCHA validation failed. Secret key not found.",
                    new[] { validationContext.MemberName });
            }

            var httpClient = new HttpClient();
            var content = new FormUrlEncodedContent(
                new[]
                    {
                        new KeyValuePair<string, string>("secret", configuration["GoogleReCaptchaV3Settings:Secret"]),
                        new KeyValuePair<string, string>("response", value.ToString()),
                    //// new KeyValuePair<string, string>("remoteip", remoteIp),
                    });

            var httpResponse = httpClient.PostAsync(configuration["GoogleReCaptchaV3Settings:ApiUrl"], content)
                .GetAwaiter().GetResult();
            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                return new ValidationResult(
                    $"Google reCAPTCHA validation failed. Status code: {httpResponse.StatusCode}.",
                    new[] { validationContext.MemberName });
            }

            var jsonResponse = httpResponse.Content.ReadAsStringAsync().Result;
            var siteVerifyResponse = JsonSerializer.Deserialize<ReCaptchaVerificationResponse>(jsonResponse);
            return siteVerifyResponse.Success
                       ? ValidationResult.Success
                       : new ValidationResult(
                           "Google reCAPTCHA validation failed.",
                           new[] { validationContext.MemberName });
        }

        //private readonly IOptions<ReCaptchaSettings> googleReCaptchaSettings;

        //public GoogleReCaptchaValidationAttribute(IOptions<ReCaptchaSettings> googleReCaptchaSettings)
        //{
        //    this.googleReCaptchaSettings = googleReCaptchaSettings;
        //}

        //protected override ValidationResult IsValid(
        //    object value,
        //    ValidationContext validationContext)
        //{
        //    if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        //    {
        //        return new ValidationResult(
        //            "Google reCAPTCHA validation failed. Value is null or empty.",
        //            new[] { validationContext.MemberName });
        //    }

        //    if (string.IsNullOrWhiteSpace(this.googleReCaptchaSettings.Value.Secret))
        //    {
        //        return new ValidationResult(
        //            "Google reCAPTCHA validation failed. Secret key not found.",
        //            new[] { validationContext.MemberName });
        //    }

        //    var httpClient = new HttpClient();
        //    var content = new FormUrlEncodedContent(
        //        new[]
        //            {
        //                new KeyValuePair<string, string>("secret", this.googleReCaptchaSettings.Value.Secret),
        //                new KeyValuePair<string, string>("response", value.ToString()),
        //            //// new KeyValuePair<string, string>("remoteip", remoteIp),
        //            });

        //    var httpResponse = httpClient.PostAsync(this.googleReCaptchaSettings.Value.ApiUrl, content)
        //        .GetAwaiter().GetResult();
        //    if (httpResponse.StatusCode != HttpStatusCode.OK)
        //    {
        //        return new ValidationResult(
        //            $"Google reCAPTCHA validation failed. Status code: {httpResponse.StatusCode}.",
        //            new[] { validationContext.MemberName });
        //    }

        //    var jsonResponse = httpResponse.Content.ReadAsStringAsync().Result;
        //    var siteVerifyResponse = JsonSerializer.Deserialize<ReCaptchaVerificationResponse>(jsonResponse);
        //    return siteVerifyResponse.Success
        //               ? ValidationResult.Success
        //               : new ValidationResult(
        //                   "Google reCAPTCHA validation failed.",
        //                   new[] { validationContext.MemberName });

        //var result = false;

        //try
        //{
        //    using var client = new HttpClient();

        //    var response = client
        //        .PostAsync(
        //        $"{this.googleReCaptchaSettings.Value.ApiUrl}?secret={this.googleReCaptchaSettings.Value.Secret}&response={token}", null)
        //         .GetAwaiter().GetResult();
        //    var jsonResponse = response.Content.ReadAsStringAsync().Result;
        //    var captchaVerfication = JsonSerializer.Deserialize<ReCaptchaVerificationResponse>(jsonResponse);

        //    if (captchaVerfication.Success && captchaVerfication.Score > 0.5)
        //    {
        //    }

        //    result = captchaVerfication.Success;
        //}
        //catch (Exception e)
        //{
        //    // TODO: fail gracefully, but log
        //    throw new Exception("Failed to process captcha validation", e);
        //}

        //return result;
    }
}
