using ASD.ViewModels;
using ASD.Views.Default;
using ASD.Views.TemplatedControls;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ASD.Views.InpaintControls;

public partial class InpaintView : BaseControl, IGetMask
{
    private Mask _horizontalMask;
    private Mask _verticalMask;
    private MainViewModel _vm;

    public InpaintView()
    {
        InitializeComponent();
        SizeChanged += SetSize;
    }

    protected override void OnLoaded()
    {
        base.OnLoaded();
        if (_vm != null) return;
        _horizontalMask = this.FindControl<Mask>("HorizontalMask");
        _verticalMask = this.FindControl<Mask>("VerticalMask");
        _vm = DataContext as MainViewModel;
        _vm.SetMask(this);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public string GetMask()
    {
        var mask = IsHorisontal ? _horizontalMask : _verticalMask;
        return mask.GetMask();
    }
}