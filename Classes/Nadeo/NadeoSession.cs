using System.Text.Json.Serialization;

public partial class NadeoSession
{
    [JsonPropertyName("platformType")]
    public string PlatformType { get; set; }

    [JsonPropertyName("ticket")]
    public string Ticket { get; set; }

    [JsonPropertyName("twoFactorAuthenticationTicket")]
    public object TwoFactorAuthenticationTicket { get; set; }

    [JsonPropertyName("profileId")]
    public string ProfileId { get; set; }

    [JsonPropertyName("userId")]
    public string UserId { get; set; }

    [JsonPropertyName("nameOnPlatform")]
    public string NameOnPlatform { get; set; }

    [JsonPropertyName("environment")]
    public string Environment { get; set; }

    [JsonPropertyName("expiration")]
    public string Expiration { get; set; }

    [JsonPropertyName("spaceId")]
    public string SpaceId { get; set; }

    [JsonPropertyName("clientIp")]
    public string ClientIp { get; set; }

    [JsonPropertyName("clientIpCountry")]
    public string ClientIpCountry { get; set; }

    [JsonPropertyName("serverTime")]
    public string ServerTime { get; set; }

    [JsonPropertyName("sessionId")]
    public string SessionId { get; set; }

    [JsonPropertyName("sessionKey")]
    public string SessionKey { get; set; }

    [JsonPropertyName("rememberMeTicket")]
    public object RememberMeTicket { get; set; }
}