using Newtonsoft.Json;

namespace RedditTokenReceiver
{
    [Serializable]
    public class Token
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}