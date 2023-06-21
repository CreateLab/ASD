using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using ASD.Consts;
using ASD.Dto;
using ASD.Models;
using ASD.Servises.ImageDimensions;
using DynamicData;
using Flurl;
using Flurl.Http;
using ReactiveUI;

namespace ASD.ViewModels;

public class MainViewModel : ViewModelBase
{
    private bool _isSetupEnd = false;

    private string _positivePrompt;
    private string _negativePrompt;
    private int _count;

    private bool _isSquare = true;
    private bool _isPortrait;
    private bool _isLandscape;

    private bool _justResize = true;
    private bool _cropAndResize;
    private bool _resizeAndFill;
    private bool _resizeAndUpscale;

    private string _imgFromImg2Img;

    private SDModel _selectedSDModel;


    private int _imgFromImg2ImgWidth;
    private int _imgFromImg2ImgHeight;

    private double _denoisingStrength = 0.75;

    public ObservableCollection<ImageModel> Images { get; set; } = new();
    public ObservableCollection<SDModel> Models { get; set; } = new();

    public string PositivePrompt
    {
        get => _positivePrompt;
        set => this.RaiseAndSetIfChanged(ref _positivePrompt, value);
    }

    public string NegativePrompt
    {
        get => _negativePrompt;
        set => this.RaiseAndSetIfChanged(ref _negativePrompt, value);
    }

    public int Count
    {
        get => _count;
        set => this.RaiseAndSetIfChanged(ref _count, value);
    }

    public bool IsSquare
    {
        get => _isSquare;
        set => this.RaiseAndSetIfChanged(ref _isSquare, value);
    }

    public bool IsPortrait
    {
        get => _isPortrait;
        set => this.RaiseAndSetIfChanged(ref _isPortrait, value);
    }

    public bool IsLandscape
    {
        get => _isLandscape;
        set => this.RaiseAndSetIfChanged(ref _isLandscape, value);
    }

    public SDModel SelectedSDModel
    {
        get => _selectedSDModel;
        set => this.RaiseAndSetIfChanged(ref _selectedSDModel, value);
    }

    public string ImgFromImg2Img
    {
        get => _imgFromImg2Img;
        set => this.RaiseAndSetIfChanged(ref _imgFromImg2Img, value);
    }

    public int ImgFromImg2ImgWidth
    {
        get => _imgFromImg2ImgWidth;
        set => this.RaiseAndSetIfChanged(ref _imgFromImg2ImgWidth, value);
    }

    public int ImgFromImg2ImgHeight
    {
        get => _imgFromImg2ImgHeight;
        set => this.RaiseAndSetIfChanged(ref _imgFromImg2ImgHeight, value);
    }
    
    public bool JustResize
    {
        get => _justResize;
        set => this.RaiseAndSetIfChanged(ref _justResize, value);
    }
    
    public bool CropAndResize
    {
        get => _cropAndResize;
        set => this.RaiseAndSetIfChanged(ref _cropAndResize, value);
    }

    public bool ResizeAndFill
    {
        get => _resizeAndFill;
        set => this.RaiseAndSetIfChanged(ref _resizeAndFill, value);
    }
    
    public bool ResizeAndUpscale
    {
        get => _resizeAndUpscale;
        set => this.RaiseAndSetIfChanged(ref _resizeAndUpscale, value);
    }
    
    public double DenoisingStrength
    {
        get => _denoisingStrength;
        set => this.RaiseAndSetIfChanged(ref _denoisingStrength, value);
    }
    
    public ReactiveCommand<Unit, Unit> GenerateImages { get; }
    public ReactiveCommand<int, Unit> SaveImage { get; }
    private ReactiveCommand<Unit, Unit> Setup { get; }
    public ReactiveCommand<int, Unit> SendToImg2Img { get; }

    public ReactiveCommand<Unit, Unit> LoadImage { get; }
    
    public ReactiveCommand<Unit,Unit> GenerateImagesFromImage { get; }
    private ReactiveCommand<Unit, Unit> SetOptions { get; }


    public MainViewModel()
    {
        GenerateImages = ReactiveCommand.CreateFromTask(GenerateImagesAsync);
        SaveImage = ReactiveCommand.CreateFromTask<int>(SaveImageAsync);
        Setup = ReactiveCommand.CreateFromTask(SetupAsync);
        SetOptions = ReactiveCommand.CreateFromTask(SetOptionsAsync);
        SendToImg2Img = ReactiveCommand.Create<int>(SendToImg2ImgMethod);
        LoadImage = ReactiveCommand.CreateFromTask(LoadImageAsync);
        GenerateImagesFromImage = ReactiveCommand.CreateFromTask(GenerateImagesFromImageAsync);
        IsSquare = true;
        Setup.Execute().Subscribe();

        this.WhenAnyValue(x => x.SelectedSDModel)
            .Where(value => value != null && _isSetupEnd) // Optional: Only trigger when the property is not empty
            .Subscribe(_ => SetOptions.Execute().Subscribe());
    }

    private async Task GenerateImagesFromImageAsync()
    {
        var resizeMode = 0;
        
        if (JustResize)
            resizeMode = 0;
        else if (CropAndResize)
            resizeMode = 1;
        else if (ResizeAndFill)
            resizeMode = 2;
        else if (ResizeAndUpscale)
            resizeMode = 3;
        
        var imgResponce = new ImgToImgDtoRequest
        {
            Height = ImgFromImg2ImgHeight,
            Width = ImgFromImg2ImgWidth,
            InitImages = new List<string>{ ImgFromImg2Img },
            Prompt =  PositivePrompt,
            NegativePrompt = NegativePrompt,
            DenoisingStrength = DenoisingStrength,
            ResizeMode = resizeMode
        };

        try
        {

     
        var result = await $"{UrlContst.ServerUrl}{UrlContst.Img2ImgUrl}".PostJsonAsync(imgResponce).ReceiveJson<Txt2ImgDtoResponse>();
        Images.Clear();
        Images.AddRange(result.images.Select((x, i) => new ImageModel
        {
            Base64Image = x,
            Id = i
        }));
        }
        catch (Exception e)
        {
            throw;
        }
    }

    private async Task LoadImageAsync()
    {
        var image = await App.Loader?.LoadImageAsBase64();
        if (image == null)
            return;
        ImgFromImg2Img = image;

        var isSuccess = ImageHelper.TryGetDimensions(ImgFromImg2Img, out var size);
        if (isSuccess)
        {
            ImgFromImg2ImgWidth = size.width;
            ImgFromImg2ImgHeight = size.height;
        }
        else
        {
            ImgFromImg2ImgWidth = 512;
            ImgFromImg2ImgHeight = 512;
        }
    }

    private void SendToImg2ImgMethod(int obj)
    {
        ImgFromImg2Img = Images[obj].Base64Image;
        var isSuccess = ImageHelper.TryGetDimensions(ImgFromImg2Img, out var size);
        if (isSuccess)
        {
            ImgFromImg2ImgWidth = size.width;
            ImgFromImg2ImgHeight = size.height;
        }
        else
        {
            ImgFromImg2ImgWidth = 512;
            ImgFromImg2ImgHeight = 512;
        }
    }

    private async Task SetOptionsAsync()
    {
        var options = new OptionsDtoRequest
        {
            SdModelCheckpoint = SelectedSDModel.Title
        };
        await $"{UrlContst.ServerUrl}{UrlContst.OptionsUrl}".PostJsonAsync(options);
    }

    private async Task SetupAsync()
    {
        var options = await $"{UrlContst.ServerUrl}{UrlContst.OptionsUrl}".GetJsonAsync<OptionsDtoResponse>();
        var models = await $"{UrlContst.ServerUrl}{UrlContst.SdModelUrl}".GetJsonAsync<ModelDtoResponse[]>();
        Models.Clear();
        Models.AddRange(models.Select(x => new SDModel
            {
                Name = x.ModelName,
                Title = x.Title
            })
            .ToList());

        SelectedSDModel = Models.FirstOrDefault(x => x.Title == options.SdModelCheckpoint);
        _isSetupEnd = true;
    }

    private async Task SaveImageAsync(int id)
    {
        await App.Saver.SaveImage("image.png", Convert.FromBase64String(Images[id].Base64Image),
            CancellationToken.None);
    }

    private async Task GenerateImagesAsync(CancellationToken token = default)
    {
        try
        {
            if (string.IsNullOrEmpty(_positivePrompt)) return;


            var model = new Txt2ImgDtoRequest(_positivePrompt, _negativePrompt);

            if (IsLandscape)
            {
                model.height = 512;
                model.width = 720;
            }
            else if (IsPortrait)
            {
                model.height = 720;
                model.width = 512;
            }
            else
            {
                model.height = 512;
                model.width = 512;
            }

            var url = $"{UrlContst.ServerUrl}{UrlContst.Txt2ImgUrl}";
            var result = await url.PostJsonAsync(model, token).ReceiveJson<Txt2ImgDtoResponse>();
            Images.Clear();
            Images.AddRange(result.images.Select((x, i) => new ImageModel
            {
                Base64Image = x,
                Id = i
            }));
        }
        catch (Exception e)
        {
            throw;
        }
    }
}