using GenshinWoodmen.Models;
using System;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace GenshinWoodmen.Core;

public static class LanguageManager
{
    public static string SystemLanguage { get; } = (CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Clone() as string)!;

    public static void SetupLanguage()
    {
        if (!string.IsNullOrWhiteSpace(Settings.Language))
        {
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture = new CultureInfo(Settings.Language);
        }
#if DEBUG
#if MUI_ZH
        Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-cn");
#elif MUI_JP
        Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture = new CultureInfo("jp");
#elif MUI_EN
        Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");
#endif
#endif
        _ = SetLanguage();
    }

    public static bool SetLanguage() => SystemLanguage switch
    {
        "zh" => SetLanguage("zh-cn"),
        "jp" => SetLanguage("jp"),
        "en" or _ => SetLanguage("en-us"),
    };

    public static bool SetLanguage(string name = "en-us")
    {
        try
        {
            foreach (ResourceDictionary dictionary in Application.Current.Resources.MergedDictionaries)
            {
                if (dictionary.Source != null && dictionary.Source.OriginalString.Equals($"/Resources/Languages/{name}.xaml"))
                {
                    Application.Current.Resources.MergedDictionaries.Remove(dictionary);
                    Application.Current.Resources.MergedDictionaries.Add(dictionary);
                    return true;
                }
            }
        }
        catch (Exception e)
        {
            _ = e;
        }
        return false;
    }

    public static string Mui(string key)
    {
        try
        {
            if (App.Current!.FindResource(key) is string value)
            {
                return value;
            }
        }
        catch (Exception e)
        {
            _ = e;
        }
        return null!;
    }

    public static string Mui(string key, string arg0)
    {
        return string.Format(Mui(key), arg0);
    }

    public static string Mui(string key, string arg0, string arg1)
    {
        return string.Format(Mui(key), arg0, arg1);
    }
}
