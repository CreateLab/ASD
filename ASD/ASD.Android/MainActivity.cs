using Android.App;
using Android.Content.PM;
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
    }
}