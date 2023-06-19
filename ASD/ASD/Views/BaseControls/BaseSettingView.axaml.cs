using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ASD.Views.BaseControls;

public partial class BaseSettingView : UserControl
{
    public BaseSettingView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}