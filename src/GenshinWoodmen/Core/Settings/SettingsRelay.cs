using GenshinWoodmen.Models;

namespace GenshinWoodmen.Core;

internal static class SettingsRelay
{
    public static void Default()
    {
        Settings.DelayLaunchFirst.Relay();
        Settings.DelayLoginFirst.Relay();
        Settings.DelayLaunch.Relay();
        Settings.DelayLogin.Relay();
        Settings.ShortcutKey.Relay();
        Settings.Language.Relay();
    }
}
