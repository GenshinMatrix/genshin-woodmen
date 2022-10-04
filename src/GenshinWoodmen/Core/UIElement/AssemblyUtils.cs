using System;
using System.Reflection;
using System.Text;

namespace GenshinWoodmen.Core
{
    internal static class AssemblyUtils
    {
        public static string GetAssemblyVersion(this Assembly assembly, VersionType type = VersionType.Major | VersionType.Minor | VersionType.Build, string prefix = null, string subfix = null)
        {
            Version version = assembly.GetName().Version!;
            StringBuilder sb = new();

            if (prefix != null)
            {
                sb.Append(prefix);
            }
            if (type.HasFlag(VersionType.Major))
            {
                sb.Append(version!.Major);
            }
            if (type.HasFlag(VersionType.Minor))
            {
                if (sb.Length > 0)
                    sb.Append(".");
                sb.Append(version!.Minor);
            }
            if (type.HasFlag(VersionType.Build))
            {
                if (sb.Length > 0)
                    sb.Append(".");
                sb.Append(version.Build);
            }
            if (type.HasFlag(VersionType.Revision))
            {
                if (sb.Length > 0)
                    sb.Append(".");
                sb.Append(version.Revision);
            }
            if (subfix != null)
            {
                sb.Append(subfix);
            }
            return sb.ToString();
        }

        [Flags]
        public enum VersionType
        {
            Major = 0,
            Minor = 1,
            Build = 2,
            Revision = 4,
        }
    }
}
