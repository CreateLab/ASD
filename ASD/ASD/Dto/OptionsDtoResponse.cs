using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace ASD.Dto;

public class OptionsDtoResponse

{
    [JsonPropertyName("samples_save")]
    [JsonProperty("samples_save")]
    public bool? SamplesSave { get; set; }

    [JsonPropertyName("samples_format")]
    [JsonProperty("samples_format")]
    public string SamplesFormat { get; set; }

    [JsonPropertyName("sd_model_checkpoint")]
    [JsonProperty("sd_model_checkpoint")]
    public string SdModelCheckpoint { get; set; }

    [JsonPropertyName("sd_vae")]
    [JsonProperty("sd_vae")]
    public string SdVae { get; set; }

    [JsonPropertyName("sd_vae_as_default")]
    [JsonProperty("sd_vae_as_default")]
    public bool? SdVaeAsDefault { get; set; }
}