using System.Text.Json.Serialization;

public partial class NadeoMapInfos
{
    [JsonPropertyName("author")]
    public string Author { get; set; }

    [JsonPropertyName("authorScore")]
    public long AuthorScore { get; set; }

    [JsonPropertyName("bronzeScore")]
    public long BronzeScore { get; set; }

    [JsonPropertyName("collectionName")]
    public string CollectionName { get; set; }

    [JsonPropertyName("createdWithGamepadEditor")]
    public object CreatedWithGamepadEditor { get; set; }

    [JsonPropertyName("createdWithSimpleEditor")]
    public object CreatedWithSimpleEditor { get; set; }

    [JsonPropertyName("filename")]
    public string Filename { get; set; }

    [JsonPropertyName("goldScore")]
    public long GoldScore { get; set; }

    [JsonPropertyName("isPlayable")]
    public bool IsPlayable { get; set; }

    [JsonPropertyName("mapId")]
    public string MapId { get; set; }

    [JsonPropertyName("mapStyle")]
    public string MapStyle { get; set; }

    [JsonPropertyName("mapType")]
    public string MapType { get; set; }

    [JsonPropertyName("mapUid")]
    public string MapUid { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("silverScore")]
    public long SilverScore { get; set; }

    [JsonPropertyName("submitter")]
    public string Submitter { get; set; }

    [JsonPropertyName("timestamp")]
    public string Timestamp { get; set; }

    [JsonPropertyName("fileUrl")]
    public Uri FileUrl { get; set; }

    [JsonPropertyName("thumbnailUrl")]
    public Uri ThumbnailUrl { get; set; }
}