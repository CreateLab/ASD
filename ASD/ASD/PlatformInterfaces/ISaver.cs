using System.Threading;
using System.Threading.Tasks;

namespace ASD.PlatformInterfaces;

public interface ISaver
{
    Task SaveImage(string fileName, byte[] imageBytes, CancellationToken cancellationToken);
}