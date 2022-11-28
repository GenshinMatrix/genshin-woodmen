using System;
using System.Globalization;
using System.Windows.Data;

namespace GenshinWoodmen.Views;

internal class EqualsConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value.Equals(parameter);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return !value.Equals(parameter);
    }
}
