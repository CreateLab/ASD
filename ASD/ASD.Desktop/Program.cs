﻿using System;
using Avalonia;
using Avalonia.ReactiveUI;

namespace ASD.Desktop;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        App.SetSaver(new Impl.Saver());
        App.SetLoader(new Impl.Loader());
        App.SetSetting(new Impl.Setting());
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
        
       
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
}