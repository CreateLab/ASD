using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace ASD.Dto;

public class OptionsDtoRequest
{
    [JsonPropertyName("sd_model_checkpoint")]
    [JsonProperty("sd_model_checkpoint")]
    public string SdModelCheckpoint { get; set; }
}