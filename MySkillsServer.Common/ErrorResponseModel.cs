namespace MySkillsServer.Web.Common
{
    using System.Text.Json.Serialization;

    public class ErrorResponseModel
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
