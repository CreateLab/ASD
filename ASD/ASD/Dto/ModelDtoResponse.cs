using System.Text.Json.Serialization;

namespace ASD.Dto;

public class ModelDtoResponse
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("model_name")]
    public string ModelName { get; set; }

    [JsonPropertyName("hash")]
    public string Hash { get; set; }

    [JsonPropertyName("sha256")]
    public string Sha256 { get; set; }

    [JsonPropertyName("filename")]
    public string Filename { get; set; }

    [JsonPropertyName("config")]
    public object Config { get; set; }
}
