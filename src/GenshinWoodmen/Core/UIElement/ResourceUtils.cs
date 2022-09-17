using System;
using System.IO;
using System.IO.Packaging;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Resources;

namespace GenshinWoodmen.Core
{
    public static class ResourceUtils
    {
        static ResourceUtils()
        {
            if (!UriParser.IsKnownScheme("pack"))
                _ = PackUriHelper.UriSchemePack;
        }

        public static byte[] GetBytes(string uriString)
        {
            Uri uri = new(uriString);
            StreamResourceInfo info = Application.GetResourceStream(uri);
            using BinaryReader stream = new(info.Stream);
            return stream.ReadBytes((int)info.Stream.Length);
        }

        public static string GetMD5(byte[] data)
        {
            using MD5 provider = MD5.Create();
            byte[] output = provider.ComputeHash(data);

            return BitConverter.ToString(output).Replace("-", "");
        }
    }
}
