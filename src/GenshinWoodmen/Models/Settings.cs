using GenshinWoodmen.Core;
using System.Reflection;

namespace GenshinWoodmen.Models
{
    [Obfuscation]
    public class Settings
    {
        public static SettingsDefinition<int> DelayLaunchFirst { get; } = new(nameof(DelayLaunchFirst), 30000);
        public static SettingsDefinition<int> DelayLoginFirst { get; } = new(nameof(DelayLoginFirst), 30000);
        public static SettingsDefinition<int> DelayLaunch { get; } = new(nameof(DelayLaunch), 18000);
        public static SettingsDefinition<int> DelayLogin { get; } = new(nameof(DelayLogin), 18000);
        public static SettingsDefinition<string> ShortcutKey { get; } = new(nameof(ShortcutKey), "F11");
        public static SettingsDefinition<string> Language { get; } = new(nameof(Language), string.Empty);
    }
}
