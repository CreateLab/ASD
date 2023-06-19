using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Media;
using Android.OS;
using ASD.PlatformInterfaces;

namespace ASD.Android.Impl;

public class Saver: ISaver
{
    public async Task SaveImage(string fileName, byte[] imageBytes, CancellationToken cancellationToken)
    {
        
        var galleryPath = Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures)?.AbsolutePath;

        // Create the directory if it doesn't exist
        Directory.CreateDirectory(galleryPath);

        // Create the file path
        string filePath = Path.Combine(galleryPath, fileName);

        // Save the image file
        await File.WriteAllBytesAsync(filePath, imageBytes,cancellationToken);

        // Notify the media scanner to scan the saved file
        MediaScannerConnection.ScanFile(Application.Context, new string[] { filePath }, null, null);
    }
}