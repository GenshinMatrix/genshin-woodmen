using System;
using System.Globalization;
using System.Windows.Data;

namespace GenshinWoodmen.Views;

internal class CountSettingsCaseToBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is CountSettingsCase @case)
        {
            return @case == (CountSettingsCase)int.Parse(parameter.ToString()!);
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool isChecked = (bool)value;

        if (!isChecked)
        {
            return CountSettingsCase.ConverterIgnore;
        }
        return (CountSettingsCase)int.Parse(parameter.ToString()!);
    }
}

public enum CountSettingsCase
{
    ConverterIgnore = -1,
    Notification = 0,
    CloseGame = 1,
    Shutdown = 2,
    Dadada = 3,
    Customize = 4,
}
