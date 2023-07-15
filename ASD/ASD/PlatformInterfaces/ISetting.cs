using System.Threading.Tasks;

namespace ASD.PlatformInterfaces;

public interface ISetting
{
    Task SaveSetting(string url, string apiKey);
    Task<(string Url, string ApiKey)?> LoadSetting();
}