using System;
using System.Windows.Data;

namespace GenshinWoodmen.Views;

internal class AutoMuteToBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value is AutoMuteSelection autoMute)
        {
            return autoMute == (AutoMuteSelection)int.Parse(parameter.ToString()!);
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        bool isChecked = (bool)value;

        if (!isChecked)
        {
            return AutoMuteSelection.ConverterIgnore;
        }
        return (AutoMuteSelection)int.Parse(parameter.ToString()!);
    }
}

public enum AutoMuteSelection
{
    ConverterIgnore = -1,
    AutoMuteOff = 0,
    AutoMuteGame,
    AutoMuteSystem,
}
