using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GenshinWoodmen.Views
{
    internal class CountSettingsCaseIndicateForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CountSettingsCase @case && @case == CountSettingsCase.Notification)
            {
                return Brushes.Black;
            }
            return new BrushConverter().ConvertFrom("#0078D4")!;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
