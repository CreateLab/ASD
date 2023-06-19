using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ASD.Views.BaseControls;

public partial class PromptView : UserControl
{
    public PromptView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}