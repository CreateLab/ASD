using Android.App;
using Android.Content.PM;
using Android.OS;
using Avalonia.Android;

namespace ASD.Android;

[Activity(Label = "ASD.Android", Theme = "@style/MyTheme.NoActionBar", Icon = "@drawable/icon",
    LaunchMode = LaunchMode.SingleTop,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity
{
    public MainActivity()
    {
        
        App.SetSaver(new Impl.Saver());
        App.SetLoader(new Impl.Loader());
    }

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        Xamarin.Essentials.Platform.Init(this, savedInstanceState);
    }
}