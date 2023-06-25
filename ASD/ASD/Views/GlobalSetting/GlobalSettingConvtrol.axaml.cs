using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ASD.Views.GlobalSetting;

public partial class GlobalSettingConvtrol : UserControl
{
    public GlobalSettingConvtrol()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}