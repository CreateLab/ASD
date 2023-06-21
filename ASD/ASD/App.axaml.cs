using System;
using ASD.PlatformInterfaces;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ASD.ViewModels;
using ASD.Views;

namespace ASD;

public partial class App : Application
{
    internal static ISaver? Saver { get; private set; }
    internal static ILoader? Loader { get; private set; }

    public static void SetSaver(ISaver saver)
    {
        if(Saver == null)
            Saver = saver;
    }
    
    public static void SetLoader(ILoader loader)
    {
        if(Loader == null)
            Loader = loader;
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel()
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}