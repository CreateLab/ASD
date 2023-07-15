using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ASD.PlatformInterfaces;

namespace ASD.Desktop.Impl;

public class Setting : ISetting
{
    public async Task SaveSetting(string url, string apiKey)
    {
        var userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var asdFolder = Path.Combine(userFolder, "ASD");
        if (!Directory.Exists(asdFolder))
        {
            Directory.CreateDirectory(asdFolder);
        }

        var filePath = Path.Combine(asdFolder, "setting.json");
        await File.WriteAllLinesAsync(filePath, new []{url,apiKey});
    }

    public async Task<(string Url, string ApiKey)?> LoadSetting()
    {
        var userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var asdFolder = Path.Combine(userFolder, "ASD");
        if (!Directory.Exists(asdFolder))
        {
            return null;
        }

        var filePath = Path.Combine(asdFolder, "setting.json");
        if (!File.Exists(filePath))
        {
            return null;
        }

        var lines = await File.ReadAllLinesAsync(filePath);

        var url = lines[0];
        var apiKey = lines[1];
        
        return (url, apiKey);
    }
}