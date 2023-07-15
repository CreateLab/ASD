using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ASD.PlatformInterfaces;


namespace ASD.Android.Impl;

public class Setting: ISetting
{
    public Task SaveSetting(string url, string apiKey)
    {
        var combine = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "setting.txt");
        return File.WriteAllTextAsync(combine, $"{{\"url\":\"{url}\",\"apiKey\":\"{apiKey}\"}}");
    }

    public async Task<(string Url, string ApiKey)?> LoadSetting()
    {
        var combine = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "setting.txt");
        if (!File.Exists(combine))
        {
            return null;
        }
        
        var json = await File.ReadAllTextAsync(combine);
        var pattern = "\"url\":\"(.*?)\",\"apiKey\":\"(.*?)\"";

// Create a Regex object with the pattern
        var regex = new Regex(pattern);

// Match the pattern against the input string
        var match = regex.Match(json);

// Extract the values
        var url = match.Groups[1].Value;
        var apiKey = match.Groups[2].Value;
        
        return (url, apiKey);
        
    }
}