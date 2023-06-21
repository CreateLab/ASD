using System.Threading;
using System.Threading.Tasks;

namespace ASD.PlatformInterfaces;

public interface ILoader
{
    Task<string> LoadImageAsBase64(CancellationToken cancellationToken = default);
}