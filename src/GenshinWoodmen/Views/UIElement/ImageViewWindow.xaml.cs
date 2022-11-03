using GenshinWoodmen.Core;
using MicaWPF.Controls;
using System;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace GenshinWoodmen.Views
{
    public partial class ImageViewWindow : MicaWindow
    {
        public ImageViewWindow()
        {
            InitializeComponent();

            imageControl.Loaded += (_, _) =>
            {
                imageCanvas.SetTransform(new(0.8d, 0d, 0d, 0.8d, (Width - imageControl.Source.Width * 0.8d) / 2d - 5, -SystemInformation.CaptionHeight - 8));
            };
        }

        public void Load(string uriString)
        {
            if (new Uri(uriString).ToBitmapSource() is BitmapSource bitmap)
            {
                imageControl.Source = bitmap;
            }
        }
    }
}
