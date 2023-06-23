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
    private int _count = 4;

    private bool _isSquare = true;
    private bool _isPortrait;
    private bool _isLandscape;

    private bool _justResize = true;
    private bool _cropAndResize;
    private bool _resizeAndFill;
    private bool _resizeAndUpscale;

    private byte[] _imgFromImg2Img;

    private SDModel _selectedSDModel;

    private ImageModel _selectedImage;

    private int _imgFromImg2ImgWidth = 512;
    private int _imgFromImg2ImgHeight = 512;

    private double _denoisingStrength = 0.75;

    private byte[] _previewImage;

    public bool _isSizeUsed;

    private string _exceptionMessage;

    private bool _isAnyException;

    private bool _isDialogOpen;
    public ObservableCollection<ImageModel> Images { get; set; } = new();
    public ObservableCollection<SDModel> Models { get; set; } = new();

    public string PositivePrompt
    {
        get => _positivePrompt;
        set => this.RaiseAndSetIfChanged(ref _positivePrompt, value);
    }

    public ImageModel SelectedImage
    {
        get => _selectedImage;
        set
        {
            PreviewImage = value.Base64Image;
            this.RaiseAndSetIfChanged(ref _selectedImage, value);
        }
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

    public byte[] ImgFromImg2Img
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

    public byte[] PreviewImage
    {
        get => _previewImage;
        set => this.RaiseAndSetIfChanged(ref _previewImage, value);
    }

    public bool IsSizeUsed
    {
        get => _isSizeUsed;
        set => this.RaiseAndSetIfChanged(ref _isSizeUsed, value);
    }

    public string ExceptionMessage
    {
        get => _exceptionMessage;
        set => this.RaiseAndSetIfChanged(ref _exceptionMessage, value);
    }

    public bool IsAnyException
    {
        get => _isAnyException;
        set => this.RaiseAndSetIfChanged(ref _isAnyException, value);
    }

    public bool IsDialogOpen
    {
        get => _isDialogOpen;
        set => this.RaiseAndSetIfChanged(ref _isDialogOpen, value);
    }

    public ReactiveCommand<Unit, Unit> GenerateImages { get; }
    public ReactiveCommand<int, Unit> SaveImage { get; }
    private ReactiveCommand<Unit, Unit> Setup { get; }
    public ReactiveCommand<int, Unit> SendToImg2Img { get; }

    public ReactiveCommand<Unit, Unit> LoadImage { get; }

    public ReactiveCommand<Unit, Unit> GenerateImagesFromImage { get; }
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

        HandleExceptions();

        IsSquare = true;
        Setup.Execute().Subscribe();

        this.WhenAnyValue(x => x.SelectedSDModel)
            .Where(value => value != null && _isSetupEnd) // Optional: Only trigger when the property is not empty
            .Subscribe(_ => SetOptions.Execute().Subscribe());
    }

    private void HandleExceptions()
    {
        var commands = new List<IReactiveCommand>
        {
            GenerateImages,
            SaveImage,
            Setup,
            SetOptions,
            SendToImg2Img,
            LoadImage,
            GenerateImagesFromImage
        };

        var combinedExceptions = commands
            .Select(command => command.ThrownExceptions)
            .Merge();

        combinedExceptions.Subscribe(exception =>
        {
            ExceptionMessage = exception.Message;
            IsAnyException = true;
            IsDialogOpen = true;
        });
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
            InitImages = new List<string> { Convert.ToBase64String(ImgFromImg2Img) },
            Prompt = PositivePrompt,
            NegativePrompt = NegativePrompt,
            DenoisingStrength = DenoisingStrength,
            ResizeMode = resizeMode
        };

        try
        {
            var result = await $"{UrlContst.ServerUrl}{UrlContst.Img2ImgUrl}".PostJsonAsync(imgResponce)
                .ReceiveJson<Txt2ImgDtoResponse>();
            Images.Clear();
            Images.AddRange(result.images.Select((x, i) => new ImageModel
            {
                Base64Image = Convert.FromBase64String(x),
                Id = i
            }));
            SelectedImage = Images.FirstOrDefault();
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
        ImgFromImg2Img = Convert.FromBase64String(image);

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
        var isSuccess = ImageHelper.TryGetDimensions(SelectedImage.Base64Image, out var size);
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

        ImgFromImg2Img = SelectedImage.Base64Image;
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
        await App.Saver.SaveImage($@"image_{Guid.NewGuid()}.png", SelectedImage.Base64Image,
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
                model.Height = 512;
                model.Width = 720;
            }
            else if (IsPortrait)
            {
                model.Height = 720;
                model.Width = 512;
            }
            else
            {
                model.Height = 512;
                model.Width = 512;
            }


            model.BatchSize = _isSizeUsed ? _count : 1;

            var iteration = !_isSizeUsed ? _count : 1;

            var url = $"{UrlContst.ServerUrl}{UrlContst.Txt2ImgUrl}";
            var imagesCollection = new List<string>();
            for (var i = 0; i < iteration; i++)
            {
                var receiveJson = await url.PostJsonAsync(model, token).ReceiveJson<Txt2ImgDtoResponse>();
                imagesCollection.AddRange(receiveJson.images);
            }


            Images.Clear();
            Images.AddRange(imagesCollection.Select((x, i) => new ImageModel
            {
                Base64Image = Convert.FromBase64String(x),
                Id = i
            }));

            SelectedImage = Images.FirstOrDefault();
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public void DoubleProportions()
    {
        ImgFromImg2ImgHeight *= 2;
        ImgFromImg2ImgWidth *= 2;
    }
}