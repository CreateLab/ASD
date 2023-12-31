﻿using Android.App;
using Android.Content;
using Android.OS;
using ASD.Android.Impl;
using Application = Android.App.Application;
using Avalonia;
using Avalonia.Android;
using Avalonia.ReactiveUI;

namespace ASD.Android;

[Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
public class SplashActivity : AvaloniaSplashActivity<App>
{
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        return base.CustomizeAppBuilder(builder)
            .WithInterFont()
            .UseReactiveUI();
    }

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        App.SetSaver(new Impl.Saver());
        App.SetLoader(new Impl.Loader());
        App.SetSetting(new Impl.Setting());
        base.OnCreate(savedInstanceState);
    }

    protected override void OnResume()
    {
        base.OnResume();

        StartActivity(new Intent(Application.Context, typeof(MainActivity)));
    }
}