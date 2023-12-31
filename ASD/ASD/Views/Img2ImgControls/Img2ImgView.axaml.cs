using ASD.Views.Default;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace ASD.Views.Img2ImgControls;

public partial class Img2ImgView : BaseControl
{
    public Img2ImgView()
    {
        InitializeComponent();
        SizeChanged += SetSize;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}