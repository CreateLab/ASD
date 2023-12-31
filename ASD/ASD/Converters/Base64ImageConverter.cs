using System;
using System.Globalization;
using System.IO;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;

namespace ASD.Converters;

public class Base64ImageConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not byte[] byteArray) return null;
     
        using var stream = new MemoryStream(byteArray);
        // Create a new BitmapImage
        var image = new Bitmap(stream);
        return image;

    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        
        throw new NotImplementedException();
    }
}