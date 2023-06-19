using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using ASD.Dto;
using ASD.Models;
using DynamicData;
using Flurl.Http;
using ReactiveUI;

namespace ASD.ViewModels;

public class MainViewModel : ViewModelBase
{
    private const string serverUrl = "http://192.168.1.163:7860";
    private string _positivePrompt;
    private string _negativePrompt;
    private int _count;

    private bool _isSquare = true;
    private bool _isPortrait;
    private bool _isLandscape;

    public ObservableCollection<ImageModel> Images { get; set; } = new();

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

    public ReactiveCommand<Unit, Unit> GenerateImages { get; }

    public ReactiveCommand<int, Unit> SaveImage { get; }

    public ReactiveCommand<Unit, Unit> Setup { get; }

    public MainViewModel()
    {
        GenerateImages = ReactiveCommand.CreateFromTask(GenerateImagesAsync);
        SaveImage = ReactiveCommand.CreateFromTask<int>(SaveImageAsync);
        Setup = ReactiveCommand.CreateFromTask(SetupAsync);
        IsSquare = true;
        Setup.Execute().Subscribe();
    }

    private async Task SetupAsync()
    {
       // throw new NotImplementedException();
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

            var url = $"{serverUrl}{Txt2ImgDtoRequest.Txt2ImgUrl}";
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