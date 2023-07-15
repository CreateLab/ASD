using System.Collections.Generic;
using Newtonsoft.Json;

namespace ASD.Dto;

public class ImgToImgDtoRequest
{
    [JsonProperty("init_images")] public List<string> InitImages { get; set; }

    [JsonProperty("resize_mode")] public int ResizeMode { get; set; }

    [JsonProperty("denoising_strength")] public double DenoisingStrength { get; set; }


    [JsonProperty("mask")] public string Mask { get; set; }

    [JsonProperty("inpainting_fill")] public int? InpaintingFill { get; set; }

    [JsonProperty("inpaint_full_res")] public bool? InpaintFullRes { get; set; }


    /*
    [JsonProperty("image_cfg_scale")]
    public int ImageCfgScale { get; set; }

  [JsonProperty("inpaint_full_res_padding")]
    public int? InpaintFullResPadding { get; set; }

    [JsonProperty("mask_blur")]
    public int MaskBlur { get; set; }

   

    [JsonProperty("inpainting_mask_invert")]
    public int InpaintingMaskInvert { get; set; }


    [JsonProperty("initial_noise_multiplier")]
    public int InitialNoiseMultiplier { get; set; }    */

    [JsonProperty("prompt")] public string Prompt { get; set; }

    /*[JsonProperty("styles")]
    public List<string> Styles { get; set; }

    [JsonProperty("seed")]
    public int Seed { get; set; }

    [JsonProperty("subseed")]
    public int Subseed { get; set; }

    [JsonProperty("subseed_strength")]
    public int SubseedStrength { get; set; }

    [JsonProperty("seed_resize_from_h")]
    public int SeedResizeFromH { get; set; }

    [JsonProperty("seed_resize_from_w")]
    public int SeedResizeFromW { get; set; }

    [JsonProperty("sampler_name")]
    public string SamplerName { get; set; }

    [JsonProperty("batch_size")]
    public int BatchSize { get; set; }

    [JsonProperty("n_iter")] public int NIter { get; set; }*/

    [JsonProperty("steps")] public int Steps { get; set; } = 20;

    /*
    [JsonProperty("cfg_scale")]
    public int CfgScale { get; set; }
    */

    [JsonProperty("width")] public int Width { get; set; }

    [JsonProperty("height")] public int Height { get; set; }

    /*
    [JsonProperty("restore_faces")]
    public bool RestoreFaces { get; set; }

    [JsonProperty("tiling")]
    public bool Tiling { get; set; }

    [JsonProperty("do_not_save_samples")]
    public bool DoNotSaveSamples { get; set; }

    [JsonProperty("do_not_save_grid")]
    public bool DoNotSaveGrid { get; set; } */

    [JsonProperty("negative_prompt")] public string NegativePrompt { get; set; }
/*
        [JsonProperty("eta")]
        public int Eta { get; set; }

        [JsonProperty("s_min_uncond")]
        public int SMinUncond { get; set; }

        [JsonProperty("s_churn")]
        public int SChurn { get; set; }

        [JsonProperty("s_tmax")]
        public int STmax { get; set; }

        [JsonProperty("s_tmin")]
        public int STmin { get; set; }

        [JsonProperty("s_noise")]
        public int SNoise { get; set; }
        */

    /*
    [JsonProperty("override_settings")]
    public OverrideSettings OverrideSettings { get; set; }
    */

    /*[JsonProperty("override_settings_restore_afterwards")]
    public bool OverrideSettingsRestoreAfterwards { get; set; }

    [JsonProperty("script_args")]
    public List<object> ScriptArgs { get; set; }

    [JsonProperty("sampler_index")]
    public string SamplerIndex { get; set; }

    [JsonProperty("include_init_images")]
    public bool IncludeInitImages { get; set; }

    [JsonProperty("script_name")]
    public string ScriptName { get; set; }

    [JsonProperty("send_images")]
    public bool SendImages { get; set; }

    [JsonProperty("save_images")]
    public bool SaveImages { get; set; }*/

    /*[JsonProperty("alwayson_scripts")]
    public AlwaysonScripts AlwaysonScripts { get; set; }*/
}