using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace ASD.Dto;

public class ModelDtoResponse
{
    [JsonProperty("title")] public string Title { get; set; }

    [JsonProperty("model_name")] public string ModelName { get; set; }

    [JsonProperty("hash")] public string Hash { get; set; }

    [JsonProperty("sha256")] public string Sha256 { get; set; }

    [JsonProperty("filename")] public string Filename { get; set; }

    [JsonProperty("config")] public object Config { get; set; }
}