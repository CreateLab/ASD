using ASD.Views.Default;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace ASD.Views.Txt2Img;

public partial class Txt2ImgView : BaseControl
{
    public Txt2ImgView()
    {
        InitializeComponent();
        SizeChanged += SetSize;
    }

   

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}