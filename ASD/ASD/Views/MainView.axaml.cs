using Avalonia.Controls;
using Avalonia.Interactivity;
using ReactiveUI;

namespace ASD.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        SizeChanged += SetSize;
       
    }

    private void SetSize(object? sender, RoutedEventArgs e)
    {
      
        var boundsWidth = this.Bounds.Width;
        var boundsHeight = this.Bounds.Height;
        if (boundsWidth > boundsHeight)
        {
            HorisontalGrid.IsVisible = true;
            VerticalGrid.IsVisible = false;
        }
        else
        {
            HorisontalGrid.IsVisible = false;
            VerticalGrid.IsVisible = true;
        }
    }
}