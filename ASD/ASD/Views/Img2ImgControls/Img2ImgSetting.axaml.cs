using System.Reactive;
using ASD.Views.BaseControls;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;

namespace ASD.Views.Img2ImgControls;

public partial class Img2ImgSetting : UserControl
{
    public Img2ImgSetting()
    {
        InitializeComponent();
        FitCommand = ReactiveCommand.Create(Fit);
    }

    private void Fit()
    {
        /*var image = this.FindControl<Image>("Image");
        var heightTextBox = this.FindControl<TextBox>("HeightTextBox");
        var widthTextBox = this.FindControl<TextBox>("WidthTextBox");
        if (image is null || heightTextBox is null || widthTextBox is null) return;
        heightTextBox.Text = image.Source.Size.Height.ToString();
        widthTextBox.Text = image.Source.Size.Width.ToString();*/
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    public static readonly AvaloniaProperty<ReactiveCommand<Unit, Unit>> FitCommandProperty =
        AvaloniaProperty.Register<ImagesView, ReactiveCommand<Unit, Unit>>(nameof(FitCommand));

    public ReactiveCommand<Unit, Unit> FitCommand
    {
        get => (ReactiveCommand<Unit, Unit>) GetValue(FitCommandProperty);
        set => SetValue(FitCommandProperty, value);
    }
}