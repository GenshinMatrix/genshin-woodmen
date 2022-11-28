using System;
using System.Globalization;
using System.Windows.Data;
using Converts = System.Convert;

namespace GenshinWoodmen.Views;

internal class AddConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double d)
        {
            try
            {
                double p = Converts.ToDouble(parameter);
                return d + p;
            }
            catch
            {
            }
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
