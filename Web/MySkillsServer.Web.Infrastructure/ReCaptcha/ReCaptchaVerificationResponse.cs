namespace MySkillsServer.Web.Infrastructure.ReCaptcha
{
    using System;
    using System.Text.Json.Serialization;

    public class ReCaptchaVerificationResponse
    {
        // reCaptcha V3 verification response is:
        // {
        //      "success": true|false,      // whether this request was a valid reCAPTCHA token for your site
        //      "score": number             // the score for this request (0.0 - 1.0)
        //      "action": string            // the action name for this request (important to verify)
        //      "challenge_ts": timestamp,  // timestamp of the challenge load (ISO format yyyy-MM-dd'T'HH:mm:ssZZ)
        //      "hostname": string,         // the hostname of the site where the reCAPTCHA was solved
        //      "error-codes": [...]        // optional
        // }
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("score")]
        public double Score { get; set; }

        [JsonPropertyName("action")]
        public string Action { get; set; }

        [JsonPropertyName("challenge_ts")]
        public DateTime ChallengeTimestamp { get; set; }

        [JsonPropertyName("hostname")]
        public string HostName { get; set; }

        [JsonPropertyName("error-codes")]
        public string[] ErrorCodes { get; set; }
    }
}
