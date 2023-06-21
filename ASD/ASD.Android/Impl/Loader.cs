using System;
using System.Threading;
using System.Threading.Tasks;
using ASD.PlatformInterfaces;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using Xamarin.Essentials;

namespace ASD.Android.Impl;

public class Loader : ILoader
{
    public async Task<string> LoadImageAsBase64(CancellationToken cancellationToken = default)
    {
        try
        {
            
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Select Image"
            });

            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                // Process the image stream as needed
                var bytes = new byte[stream.Length];
                await stream.ReadAsync(bytes, 0, (int)stream.Length, cancellationToken);
                return Convert.ToBase64String(bytes);
            }
            return null;
        }
        catch (Exception e)
        {
            throw;
        }
    }
}