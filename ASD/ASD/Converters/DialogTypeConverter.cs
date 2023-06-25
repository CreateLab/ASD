using System;
using System.Globalization;
using ASD.Enums;
using Avalonia.Data.Converters;

namespace ASD.Converters;

public class DialogTypeConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not PopupShowEnum popupShowEnum) return null;
        if (parameter is not PopupShowEnum parameterShowEnum) return null;
        return popupShowEnum == parameterShowEnum;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}