using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ASD.PlatformInterfaces;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace ASD.Desktop.Impl;

public class Loader : ILoader
{
    public async Task<string> LoadImageAsBase64(CancellationToken cancellationToken = default)
    {
        var openFileDialog = new OpenFileDialog
        {
            Title = "Open image",
            Filters = new List<FileDialogFilter>
            {
                new() { Name = "Image", Extensions = new List<string> { "png", "jpeg", "jpg" } }
            }
        };

        var filePath =
            await openFileDialog.ShowAsync(
                (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow);

        if (filePath?.Any() != true)
            return null;

        var file = await File.ReadAllBytesAsync(filePath.First(), cancellationToken);
        return Convert.ToBase64String(file);
    }
}