using GenshinWoodmen.Views;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace GenshinWoodmen.Core;

internal static class UsageManager
{
    public static string UsageImagePath { get; } = SpecialPathProvider.GetPath("usage.jpg");
    public static string UsageImagePathDirectory => new FileInfo(UsageImagePath).Directory!.FullName;

    public static async Task ShowUsageImage(UsageImageType type = UsageImageType.Normal)
    {
        try
        {
            await Task.Run(() =>
            {
                try
                {
                    byte[] image = ResourceUtils.GetBytes($"pack://application:,,,/{Pack.Name};component/Resources/usage" + type switch
                    {
                        UsageImageType.Single => "_single",
                        UsageImageType.Multi => "_multi",
                        UsageImageType.Normal or _ => string.Empty,
                    } + ".jpg");
                    string md5 = ResourceUtils.GetMD5(image);

                    if (File.Exists(UsageImagePath))
                    {
                        if (ResourceUtils.GetMD5(File.ReadAllBytes(UsageImagePath)) != md5)
                        {
                            File.WriteAllBytes(UsageImagePath, image);
                        }
                    }
                    else
                    {
                        if (!Directory.Exists(UsageImagePathDirectory))
                            Directory.CreateDirectory(UsageImagePathDirectory);
                        File.WriteAllBytes(UsageImagePath, image);
                    }
                }
                catch
                {
                }
            });
            _ = Process.Start(new ProcessStartInfo()
            {
                FileName = "cmd.exe",
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = $"/c \"{UsageImagePath}\"",
            });
        }
        catch
        {
        }
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
