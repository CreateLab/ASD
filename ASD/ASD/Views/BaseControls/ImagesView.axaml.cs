using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ASD.Views.BaseControls;

public partial class ImagesView : UserControl
{
    public ImagesView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}