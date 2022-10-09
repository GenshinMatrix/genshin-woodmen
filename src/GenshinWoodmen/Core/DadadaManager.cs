using GenshinWoodmen.Models;
using GenshinWoodmen.Views;
using ModernWpf.Controls;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GenshinWoodmen.Core
{
    internal class DadadaManager
    {
        public static string AudioPath { get; } = SpecialPathProvider.GetPath("klee_dadada.mp3");
        public static string AudioPathDirectory => new FileInfo(AudioPath).Directory!.FullName;

        public static async Task Play()
        {
            try
            {
                await Task.Run(() =>
                {
                    try
                    {
                        string lang = new CultureInfo(Settings.Language.Get() ?? "zh").TwoLetterISOLanguageName.ToLower();

                        if ((new string[] { "en", "jp", "ko" }).All(l => l != lang))
                        {
                            lang = string.Empty;
                        }
                        else
                        {
                            lang = $".{lang}";
                        }

                        byte[] audio = ResourceUtils.GetBytes($"pack://application:,,,/{Pack.Name};component/Resources/Audios/klee_dadada{lang}.mp3");
                        string md5 = ResourceUtils.GetMD5(audio);

                        if (File.Exists(AudioPath))
                        {
                            if (ResourceUtils.GetMD5(File.ReadAllBytes(AudioPath)) != md5)
                            {
                                File.WriteAllBytes(AudioPath, audio);
                            }
                        }
                        else
                        {
                            if (!Directory.Exists(AudioPathDirectory))
                                Directory.CreateDirectory(AudioPathDirectory);
                            File.WriteAllBytes(AudioPath, audio);
                        }
                    }
                    catch
                    {
                    }
                });
                AudioPlayer.Play(AudioPath);
            }
            catch
            {
            }
        }

        public static async Task Stop()
        {
            AudioPlayer.Stop(AudioPath);
            await Task.Delay(999);
            Clean();
        }

        public static async Task Show()
        {
            await Application.Current.Dispatcher.Invoke(async () =>
            {
                using DialogWindow win = new()
                {
                    Topmost = true,
                    Width = SystemParameters.WorkArea.Width,
                    Height = SystemParameters.WorkArea.Height,
                };
                win.Show();
                DadadaDialog dialog = new();
                await dialog.ShowAsync(ContentDialogPlacement.Popup);
            });
        }

        public static void Clean()
        {
            try
            {
                File.Delete(AudioPath);
            }
            catch
            {
            }
        }
    }
}
