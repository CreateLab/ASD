using System.Text.Json.Serialization;

namespace ASD.Dto;

public class OptionsDtoRequest
{
    [JsonPropertyName("sd_model_checkpoint")]
    public string SdModelCheckpoint { get; set; }
}