using System;
using System.IO;
using System.Reactive;
using System.Threading.Tasks;
using ASD.Enums;
using ASD.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using ReactiveUI;

namespace ASD.Views.BaseControls;

public partial class ImagesView : UserControl
{
    public ImagesView()
    {
        InitializeComponent();
        CopyCommand = ReactiveCommand.CreateFromTask(CopyAsync);
        SaveCommand = ReactiveCommand.CreateFromTask(SaveAsync);
    }

    private async Task SaveAsync()
    {
        var image =  this.FindControl<Image>("Image");
        Bitmap bitmap = (Bitmap)image.Source;
        if (bitmap is null) return;
        var storageFile = await TopLevel.GetTopLevel(this)?.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            SuggestedFileName = Guid.NewGuid().ToString() + ".png",
            DefaultExtension = "png",

        });
        
 
        

    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private async Task CopyAsync()
    {
        var image =  this.FindControl<Image>("Image");
        Bitmap bitmap = (Bitmap)image.Source;
        if (bitmap is null) return ;
        var clipboard = TopLevel.GetTopLevel(this)?.Clipboard;
        if (clipboard is null) return ;
        var data = new DataObject();
        using (var stream = new MemoryStream())
        {
            bitmap.Save(stream);
            stream.Seek(0, SeekOrigin.Begin);
            data.Set("Png", stream);
            await clipboard.SetDataObjectAsync(data);
        }
        var getData = await clipboard.GetDataAsync("Png");
    }

    public static readonly AvaloniaProperty<ReactiveCommand<Unit, Unit>> CopyCommandClickProperty =
        AvaloniaProperty.Register<ImagesView, ReactiveCommand<Unit, Unit>>(nameof(CopyCommand));

    public ReactiveCommand<Unit, Unit> CopyCommand
    {
        get => (ReactiveCommand<Unit, Unit>) GetValue(CopyCommandClickProperty);
        set => SetValue(CopyCommandClickProperty, value);
    }
    
    public static readonly AvaloniaProperty<ReactiveCommand<Unit, Unit>> SaveCommandProperty =
        AvaloniaProperty.Register<ImagesView, ReactiveCommand<Unit, Unit>>(nameof(SaveCommand));

    public ReactiveCommand<Unit, Unit> SaveCommand
    {
        get => (ReactiveCommand<Unit, Unit>) GetValue(SaveCommandProperty);
        set => SetValue(SaveCommandProperty, value);
    }

    private void Image_OnTapped(object? sender, TappedEventArgs e)
    {
        var mainViewModel = this.DataContext as MainViewModel;
        if (mainViewModel is null) return;
        mainViewModel.DialogType = PopupShowEnum.Image;
        mainViewModel.IsDialogOpen = true;
    }
}