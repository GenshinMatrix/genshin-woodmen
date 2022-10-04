using GenshinWoodmen.Core;

namespace GenshinWoodmen
{
    internal static class Pack
    {
        public static string Name => "GenshinWoodmen";
        public static string Alias => "genshin-woodmen";
        public static string Url => "https://github.com/genshin-matrix/genshin-woodmen/releases";
        public static string Version => AssemblyUtils.GetAssemblyVersion(typeof(App).Assembly, prefix: "v");
    }
}
