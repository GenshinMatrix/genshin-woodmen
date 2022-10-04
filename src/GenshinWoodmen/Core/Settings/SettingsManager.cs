using System;
using System.IO;

namespace GenshinWoodmen.Core
{
    internal class SettingsManager
    {
        public static event Action? Reloaded;
        public static readonly string Path = SpecialPathProvider.GetPath($"{Pack.Alias}.yaml");
        public static SettingsCache Cache = Init();

        public static void Setup()
        {
            _ = Cache;
            SettingsRelay.Default();
            Save();
        }

        public static SettingsCache Init()
        {
            SettingsCache instance = null!;

            if (File.Exists(Path))
            {
                instance = Load();
            }

            if (instance == null)
            {
                instance = new();
            }
            return instance;
        }

        public static void Reinit()
        {
            Cache = Init();
            Reloaded?.Invoke();
        }

        public static SettingsCache Load()
        {
            return LoadFrom(Path);
        }

        public static SettingsCache LoadFrom(string filename)
        {
            try
            {
                return SettingsSerializer.DeserializeFile<SettingsCache>(filename) ?? new();
            }
            catch (Exception e)
            {
                _ = e;
                return new();
            }
        }

        public static bool Save()
        {
            return SaveAs(Path);
        }

        public static bool SaveAs(string filename, bool overwrite = true)
        {
            if (!overwrite && File.Exists(filename))
            {
                return true;
            }
            return SettingsSerializer.SerializeFile(Path, Cache);
        }

        public static bool SaveAs(string filename, object obj, bool overwrite = true)
        {
            if (!overwrite && File.Exists(filename))
            {
                return true;
            }
            return SettingsSerializer.SerializeFile(filename, obj);
        }
    }
}
