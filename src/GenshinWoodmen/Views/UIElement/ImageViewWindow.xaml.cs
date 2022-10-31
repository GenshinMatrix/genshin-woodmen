using GenshinWoodmen.Core;
using MicaWPF.Controls;
using System;
using System.Windows.Media.Imaging;

namespace GenshinWoodmen.Views
{
    public partial class ImageViewWindow : MicaWindow
    {
        public ImageViewWindow()
        {
            InitializeComponent();
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
