using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace ASD.Views.Img2ImgControls;

public partial class Img2ImgView : UserControl
{
    public Img2ImgView()
    {
        InitializeComponent();
        SizeChanged += SetSize;
    }

    private void SetSize(object? sender, RoutedEventArgs e)
    {
        var horisontalGrid = this.FindControl<Grid>("HorisontalGrid");
        var verticalGrid = this.FindControl<Grid>("VerticalGrid");
        var boundsWidth = this.Bounds.Width;
        var boundsHeight = this.Bounds.Height;
        if (boundsWidth > boundsHeight)
        {
            horisontalGrid.IsVisible = true;
            verticalGrid.IsVisible = false;
        }
        else
        {
            horisontalGrid.IsVisible = false;
            verticalGrid.IsVisible = true;
        }
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}