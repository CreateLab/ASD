using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace ASD.Dto;

public class Txt2ImgDtoRequest
{
    public Txt2ImgDtoRequest(string? prompt, string? negativePrompt, int? width = null, int? height = null)
    {
        this.Prompt = prompt;
        this.NegativePrompt = negativePrompt;
        this.Width = width;
        this.Height = height;
        this.Steps = 20;
    }


    /*public bool? enable_hr { get; set; }
    public int? denoising_strength { get; set; }
    public int? firstphase_width { get; set; }
    public int? firstphase_height { get; set; }
    public int? hr_scale { get; set; }
    public string? hr_upscaler { get; set; }
    public int? hr_second_pass_steps { get; set; }
    public int? hr_resize_x { get; set; }
    public int? hr_resize_y { get; set; }
    public string? hr_sampler_name { get; set; }
    public string? hr_prompt { get; set; }
    public string? hr_negative_prompt { get; set; }*/
    [JsonProperty("prompt")]
    public string? Prompt { get; set; }

    /*public List<string?> styles { get; set; }
    public int? seed { get; set; }
    public int? subseed { get; set; }
    public int? subseed_strength { get; set; }
    public int? seed_resize_from_h { get; set; }
    public int? seed_resize_from_w { get; set; }
    public string? sampler_name { get; set; }
    
    public int? n_iter { get; set; }*/
    
    [JsonProperty("batch_size")]
    public int? BatchSize { get; set; }
    
    [JsonProperty("steps")]
    public int? Steps { get; set; }

    [JsonProperty("width")]
    [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Width { get; set; }

    [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonProperty("height")]
    public int? Height { get; set; }

    /*public int? cfg_scale { get; set; }
    public bool? restore_faces { get; set; }
    public bool? tiling { get; set; }
    public bool? do_not_save_samples { get; set; }
    public bool? do_not_save_grid { get; set; }*/
    [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonProperty("negative_prompt")]
    public string? NegativePrompt { get; set; }
    /*public int? eta { get; set; }
    public int? s_min_uncond { get; set; }
    public int? s_churn { get; set; }
    public int? s_tmax { get; set; }
    public int? s_tmin { get; set; }
    public int? s_noise { get; set; }
   // public OverrideSettings override_settings { get; set; } not implemented
    public bool? override_settings_restore_afterwards { get; set; }
    public List<object> script_args { get; set; }
    public string? sampler_index { get; set; }
    public string? script_name { get; set; }
    public bool? send_images { get; set; }
    public bool? save_images { get; set; }*/
    // public AlwaysonScripts alwayson_scripts { get; set; } not implemented
}