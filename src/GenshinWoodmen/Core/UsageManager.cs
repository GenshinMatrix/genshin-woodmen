using GenshinWoodmen.Views;
using System.IO;
using System.Threading.Tasks;

namespace GenshinWoodmen.Core;

internal static class UsageManager
{
    public static string UsageImagePath { get; } = SpecialPathProvider.GetPath("usage.jpg");
    public static string UsageImagePathDirectory => new FileInfo(UsageImagePath).Directory!.FullName;

    public static bool ShowUsageImage(UsageImageType type = UsageImageType.Normal)
    {
        string uriString = $"pack://application:,,,/{Pack.Name};component/Resources/Guides/Usage" + type switch
        {
            UsageImageType.Single => "_single",
            UsageImageType.Multi => "_multi",
            UsageImageType.Normal or _ => string.Empty,
        } + ".jpg";
        string title = type switch
        {
            UsageImageType.Single => Mui("WoodGuide1"),
            UsageImageType.Multi => Mui("WoodGuide2"),
            UsageImageType.Normal or _ => Mui("WoodGuide"),
        };

        try
        {
            ImageViewWindow win = new() { Title = title };
            win.Load(uriString);
            win.Show();
            return true;
        }
        catch
        {
        }
        return false;
    }

    public static async Task ShowUsage()
    {
        await DialogWindow.ShowMessage(Mui("Usage"), Mui("UsageMessage"));
    }

    public static void CleanUsageImage()
    {
        try
        {
            File.Delete(UsageImagePath);
        }
        catch
        {
        }
    }
}

public enum UsageImageType
{
    Normal,
    Single,
    Multi,
}
