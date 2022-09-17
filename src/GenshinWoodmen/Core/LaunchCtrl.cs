using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenshinWoodmen.Core
{
    internal class LaunchCtrl
    {
        public static async Task<bool> SearchRunning()
        {
            return await Task.Run(() =>
            {
                try
                {
                    Process[] processes = Process.GetProcessesByName("YuanShen");

                    if (processes.Length <= 0)
                    {
                        processes = Process.GetProcessesByName("Genshin Impact");
                    }
                    if (processes.Length <= 0)
                    {
                        processes = Process.GetProcessesByName("GenshinImpact");
                    }
                    return processes.Length > 0;
                }
                catch
                {
                }
                return false;
            });
        }

        public static async Task<bool> IsRunning(Func<Process?, Task> callback = null!)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    Process[] processes = Process.GetProcessesByName("YuanShen");

                    if (processes.Length <= 0)
                    {
                        processes = Process.GetProcessesByName("Genshin Impact");
                    }
                    if (processes.Length <= 0)
                    {
                        processes = Process.GetProcessesByName("GenshinImpact");
                    }
                    if (processes.Length > 0)
                    {
                        foreach (Process? process in processes)
                        {
                            await callback?.Invoke(process)!;
                            return true;
                        }
                    }
                }
                catch
                {
                }
                return false;
            });
        }

        public static async Task<bool> Close()
        {
            return await IsRunning(async p => p?.CloseMainWindow());
        }

        public static async Task<bool> Kill()
        {
            return await IsRunning(async p => p?.Kill());
        }

        public static async Task<bool> Logout()
        {
            return await IsRunning(async p =>
            {
                IntPtr hwnd = p!.MainWindowHandle;

                await JiggingProcessor.Delay(1000);
                NativeMethods.Focus(hwnd);
                NativeMethods.CursorCenterPos(hwnd);
                UserSimulator.Input.Keyboard.KeyPress(VirtualKeyCode.ESCAPE);
                await JiggingProcessor.Delay(2000);
                RECT rect = NativeMethods.GetWindowRECT(hwnd);
                NativeMethods.SetCursorPos(rect.Left + 20, rect.Bottom - SystemInformation.CaptionHeight - 10);
                await JiggingProcessor.Delay(100);
                UserSimulator.Input.Mouse.LeftButtonClick(); // ExitButton
                await JiggingProcessor.Delay(2000);
                NativeMethods.SetCursorPos(rect.Left + (int)((rect.Right - rect.Left) / 2d) + 70, rect.Top + (int)((rect.Bottom - rect.Top) / 2d) + 170);
                await JiggingProcessor.Delay(100);
                UserSimulator.Input.Mouse.LeftButtonClick(); // OKButton
            });
        }

        public static async Task<IntPtr> Launch(int? delayMs = null, RelaunchMethod relaunchMethod = RelaunchMethod.Logout)
        {
            try
            {
                if (relaunchMethod == RelaunchMethod.Kill ? await Kill() : relaunchMethod == RelaunchMethod.Close ? await Close() : await Logout())
                {
                    if (!SpinWait.SpinUntil(() => SearchRunning().Result, 15000))
                    {
                        NoticeService.AddNotice(Pack.Url, "Failed", "Failed to kill Genshin Impact.");
                        return IntPtr.Zero;
                    }
                }
            }
            catch (Exception e)
            {
                NoticeService.AddNotice(Pack.Name, "Failed", e.Message);
            }

            if (string.IsNullOrEmpty(GenshinRegedit.InstallPath))
            {
                NoticeService.AddNotice(Pack.Url, "Failed", "Genshin Impact not installed.");
            }
            else
            {
                if (delayMs != null)
                    await JiggingProcessor.Delay((int)delayMs);
                Process? p = Process.Start(new ProcessStartInfo()
                {
                    UseShellExecute = true,
                    FileName = Path.Combine(GenshinRegedit.InstallPath, "Genshin Impact Game", "YuanShen.exe"),
                    Arguments = string.Empty,
                    WorkingDirectory = Environment.CurrentDirectory,
                    Verb = "runas",
                });
                return p?.MainWindowHandle ?? IntPtr.Zero;
            }
            return IntPtr.Zero;
        }

        public enum RelaunchMethod
        {
            Kill,
            Close,
            Logout,
        }
    }
}
