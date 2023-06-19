using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ASD.PlatformInterfaces;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace ASD.Desktop.Impl;

public class Saver : ISaver
{
    public async Task SaveImage(string fileName, byte[] imageBytes, CancellationToken cancellationToken)
    {
        var saveFileDialog = new SaveFileDialog
        {
            Title = "Save image",
            InitialFileName = fileName,
            Filters = new List<FileDialogFilter>
            {
                new() { Name = "Image", Extensions = new List<string> { "png" } }
            }
        };

        var filePath =
            await saveFileDialog.ShowAsync(
                (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow);

        if (filePath != null)
        {
            await File.WriteAllBytesAsync(filePath, imageBytes, cancellationToken);
        }
    }
}