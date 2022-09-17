using System;
using System.IO;

namespace GenshinWoodmen.Core
{
    internal class SpecialPathProvider
    {
        public static string TempPath { get; } = Path.GetTempPath();

        public static string GetPath(string baseName)
        {
            string appUserPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string configPath = Path.Combine(Path.Combine(appUserPath, Pack.Name), baseName);

            if (!Directory.Exists(new FileInfo(configPath).DirectoryName))
            {
                Directory.CreateDirectory(new FileInfo(configPath).DirectoryName!);
            }
            return configPath;
        }

        public static string GetTempPath(string baseName)
        {
            return Path.Combine(TempPath + Pack.Name, baseName);
        }
    }
}
