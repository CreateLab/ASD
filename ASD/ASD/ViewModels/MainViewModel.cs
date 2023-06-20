using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using ASD.Consts;
using ASD.Dto;
using ASD.Models;
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

    private SDModel _selectedSDModel;

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

    public ReactiveCommand<Unit, Unit> GenerateImages { get; }

    public ReactiveCommand<int, Unit> SaveImage { get; }

    private ReactiveCommand<Unit, Unit> Setup { get; }

    private ReactiveCommand<Unit, Unit> SetOptions { get; }

    public MainViewModel()
    {
        GenerateImages = ReactiveCommand.CreateFromTask(GenerateImagesAsync);
        SaveImage = ReactiveCommand.CreateFromTask<int>(SaveImageAsync);
        Setup = ReactiveCommand.CreateFromTask(SetupAsync);
        SetOptions = ReactiveCommand.CreateFromTask(SetOptionsAsync);
        IsSquare = true;
        /*Setup.Execute().Subscribe();

        this.WhenAnyValue(x => x.SelectedSDModel)
            .Where(value => value != null && _isSetupEnd) // Optional: Only trigger when the property is not empty
            .Subscribe(_ => SetOptions.Execute().Subscribe());*/
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