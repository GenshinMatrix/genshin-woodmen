using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using vccli.net;

namespace GenshinWoodmen.Core
{
    internal class MuteManager
    {
        public static async Task MuteGameAsync(bool isMuted)
        {
            await LaunchCtrl.IsRunning(async p =>
            {
                await MuteProcessAsync(p!.Id, isMuted);
            });
        }

        private static async Task MuteProcessAsync(int pid, bool isMuted)
        {
            try
            {
                if (isMuted)
                    await Api.MuteAsync(pid);
                else
                    await Api.UnmuteAsync(pid);
            }
            catch (Exception e)
            {
                Logger.Exception(e);
            }
        }

        public static void MuteSystem(bool isMuted)
        {
            try
            {
                SystemVolume.SetMasterVolumeMute(isMuted);
            }
            catch
            {
            }
        }

#if LEGACY
        [Obsolete]
        public static void MuteSystem(IntPtr? hwnd = null)
        {
            try
            {
                hwnd ??= new WindowInteropHelper(Application.Current.MainWindow).Handle;
                _ = NativeMethods.SendMessage((IntPtr)hwnd, NativeMethods.WM_APPCOMMAND, (int)hwnd, NativeMethods.APPCOMMAND_VOLUME_MUTE);
            }
            catch (Exception e)
            {
                Logger.Exception(e);
            }
        }
#endif
    }
}
