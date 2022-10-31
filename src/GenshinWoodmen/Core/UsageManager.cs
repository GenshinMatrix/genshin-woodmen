using GenshinWoodmen.Views;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace GenshinWoodmen.Core;

internal static class UsageManager
{
    public static string UsageImagePath { get; } = SpecialPathProvider.GetPath("usage.jpg");
    public static string UsageImagePathDirectory => new FileInfo(UsageImagePath).Directory!.FullName;

    public static bool ShowUsageImage(UsageImageType type = UsageImageType.Normal)
    {
        string uriString = $"pack://application:,,,/{Pack.Name};component/Resources/usage" + type switch
        {
            UsageImageType.Single => "_single",
            UsageImageType.Multi => "_multi",
            UsageImageType.Normal or _ => string.Empty,
        } + ".jpg";

        try
        {
            ImageViewWindow win = new();
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
