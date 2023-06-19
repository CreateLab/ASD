using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using ASD.Dto;
using ASD.Models;
using Flurl.Http;
using ReactiveUI;

namespace ASD.ViewModels;

public class MainViewModel : ViewModelBase
{
    private const string serverUrl = "http://192.168.1.163:7860";
    private string _positivePrompt;
    private string _negativePrompt;
    private int _count;
    
    
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
    
    public ReactiveCommand<Unit, Unit> GenerateImages { get; }
    public MainViewModel()
    {
        GenerateImages = ReactiveCommand.CreateFromTask(GenerateImagesAsync);
    }

    private async Task GenerateImagesAsync(CancellationToken token = default)
    {
        try
        {
            var model = new Txt2ImgDtoRequest(_positivePrompt, _negativePrompt);
            var url = $"{serverUrl}{Txt2ImgDtoRequest.Txt2ImgUrl}";
            var result = await url.PostJsonAsync(model, token).ReceiveJson<Txt2ImgDtoResponse>();
            Images.Clear();
            foreach (var image in result.images)
            {
                Images.Add(new ImageModel { Base64Image = image });
            }
        }
        catch (Exception e)
        {
           
            throw;
        }
    }
}