using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace GenshinWoodmen.Core;

public static class ImageExtensions
{
    public static BitmapSource ToBitmapSource(this Uri uri, int? size = null)
    {
        if (uri?.Scheme?.ToLower() == "pack")
        {
            using Stream stream = ResourceUtils.GetStream(uri);
            return stream.ToBitmapSource(size);
        }
        else
        {
            return uri?.AbsolutePath.ToBitmapSource(size) ?? null!;
        }
    }

    public static BitmapSource ToBitmapSource(this string fileSource, int? size = null)
    {
        using FileStream stream = new(fileSource, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        BitmapImage bitmapImage = new();
        bitmapImage.BeginInit();
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;

        if (size.HasValue)
            bitmapImage.DecodePixelHeight = size.Value;

        bitmapImage.StreamSource = stream;
        bitmapImage.EndInit();
        bitmapImage.Freeze();
        return bitmapImage;
    }

    public static BitmapSource ToBitmapSource(this byte[] array, int? size = null)
    {
        using MemoryStream stream = new(array);
        BitmapImage bitmapImage = new();
        bitmapImage.BeginInit();
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;

        if (size.HasValue)
            bitmapImage.DecodePixelHeight = size.Value;

        bitmapImage.StreamSource = stream;
        bitmapImage.EndInit();
        bitmapImage.Freeze();
        return bitmapImage;
    }

    public static BitmapSource ToBitmapSource(this Stream stream, int? size = null)
    {
        BitmapImage bitmapImage = new();
        bitmapImage.BeginInit();
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;

        if (size.HasValue)
            bitmapImage.DecodePixelHeight = size.Value;

        bitmapImage.StreamSource = stream;
        bitmapImage.EndInit();
        bitmapImage.Freeze();
        return bitmapImage;
    }
}
