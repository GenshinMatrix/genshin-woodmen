using System;
using System.Windows.Data;

namespace GenshinWoodmen.Views
{
    public class LanguageToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string lang)
            {
                return lang == parameter as string;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isChecked = (bool)value;

            if (!isChecked)
            {
                return string.Empty;
            }
            return (parameter as string) ?? string.Empty;
        }
    }
}
