using System;
using System.ComponentModel;
using System.IO;

namespace GenshinWoodmen.Core;

internal class SpecialPathProvider
{
    public static string TempPath { get; } = Path.GetTempPath();

    public static string GetPath(string baseName)
    {
        MigrateLegacy(baseName); // Remove this line since 2023.12.08
        return GetPathInternal(baseName);
    }

    internal static string GetPathInternal(string baseName)
    {
        string appUserPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string configPath = Path.Combine(Path.Combine(appUserPath, Pack.Alias), baseName);

        if (!Directory.Exists(new FileInfo(configPath).DirectoryName))
        {
            Directory.CreateDirectory(new FileInfo(configPath).DirectoryName!);
        }
        return configPath;
    }

    public static string GetTempPath(string baseName)
    {
        return Path.Combine(TempPath + Pack.Alias, baseName);
    }

    #region Legacy

    [Description("Legacy")]
    private static string GetPathLegacy(string baseName)
    {
        string appUserPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string configPath = Path.Combine(Path.Combine(appUserPath, Pack.Alias), baseName);

        return configPath;
    }

    [Description("Legacy")]
    private static void MigrateLegacy(string baseName)
    {
        try
        {
            string path = GetPathInternal(baseName);
            string pathLegacy = GetPathLegacy(baseName);

            if (!File.Exists(path) && File.Exists(pathLegacy))
            {
                File.Copy(pathLegacy, path);
            }
        }
        catch
        {
            Logger.Warn($"[SpecialPathProvider] Migrate Legacy file `{baseName}` failed.");
        }
    }

    #endregion Legacy
}
