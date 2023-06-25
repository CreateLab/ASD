using Avalonia.Controls;
using Avalonia.Interactivity;

namespace ASD.Views.Default;

public class BaseControl: UserControl
{
    protected bool IsHorisontal { get; set; }
    protected void SetSize(object? sender, RoutedEventArgs e)
    {
        var horisontalGrid = this.FindControl<Grid>("HorizontalGrid");
        var verticalGrid = this.FindControl<Control>("VerticalGrid");
        var boundsWidth = this.Bounds.Width;
        var boundsHeight = this.Bounds.Height;
        if (boundsWidth > boundsHeight)
        {
            horisontalGrid.IsVisible = true;
            verticalGrid.IsVisible = false;
            IsHorisontal = true;
        }
        else
        {
            horisontalGrid.IsVisible = false;
            verticalGrid.IsVisible = true;
            IsHorisontal = false;
        }
    }
}