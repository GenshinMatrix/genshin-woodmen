using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

        public static bool TwiceEsc { get; set; } = false;
        public static async Task<bool> Logout()
        {
            return await IsRunning(async p =>
            {
                IntPtr hwnd = p!.MainWindowHandle;

                NativeMethods.Focus(hwnd);
                NativeMethods.CursorCenterPos(hwnd);
                UserSimulator.Input.Keyboard.KeyPress(VirtualKeyCode.ESCAPE);
                if (TwiceEsc)
                {
                    await JiggingProcessor.Delay(600);
                    UserSimulator.Input.Keyboard.KeyPress(VirtualKeyCode.ESCAPE);
                }
                await JiggingProcessor.Delay(950);
                RECT rect = NativeMethods.GetWindowRECT(hwnd);
                NativeMethods.SetCursorPos(rect.Left + 20, rect.Bottom - 30);
                await JiggingProcessor.Delay(100);
                UserSimulator.Input.Mouse.LeftButtonClick(); // Exit
                await JiggingProcessor.Delay(800);
                (double cx, double cy) = ((rect.Right - rect.Left) / 2d, (rect.Bottom - rect.Top) / 2d);
                double ratio = (rect.Right - rect.Left) / (double)(rect.Bottom - rect.Top);
                (double xfactor, double yfactor) = (1.1d, new Func<double>(() =>
                {
                    // Inspected from ratio (such as 1.33, 1.5, 1.6, 1.78), calced without `SystemInformation.CaptionHeight` actually.
                    // TODO: Calc with `SystemInformation.CaptionHeight`.
                    if (ratio >= 1.6d) return 1.4d;
                    else if (ratio >= 1.5d) return 1.35d;
                    else if (ratio >= 1.4d) return 1.33d;
                    else return 1.3d;
                }).Invoke());
                NativeMethods.SetCursorPos(rect.Left + (int)(cx * xfactor), rect.Top + (int)(cy * yfactor));
                await JiggingProcessor.Delay(100);
                UserSimulator.Input.Mouse.LeftButtonClick(); // OK
            });
        }

        public static async Task<IntPtr> Launch(int? delayMs = null, RelaunchMethod relaunchMethod = RelaunchMethod.None, LaunchParameter launchParameter = null!)
        {
            try
            {
                if (relaunchMethod == RelaunchMethod.Kill ? await Kill() : relaunchMethod == RelaunchMethod.Close ? await Close() : relaunchMethod == RelaunchMethod.Logout ? await Logout() : false)
                {
                    if (!SpinWait.SpinUntil(() => SearchRunning().Result, 15000))
                    {
                        NoticeService.AddNotice(Mui("Tips"), "Failed", "Failed to kill Genshin Impact.");
                        return IntPtr.Zero;
                    }
                }
            }
            catch (Exception e)
            {
                NoticeService.AddNotice(Mui("Tips"), "Failed", e.Message);
            }

            if (string.IsNullOrEmpty(GenshinRegedit.InstallPath))
            {
                NoticeService.AddNotice(Mui("Tips"), "Failed", "Genshin Impact not installed.");
            }
            else
            {
                const string GameFolderName = "Genshin Impact Game";

                if (delayMs != null)
                    await JiggingProcessor.Delay((int)delayMs);

                string fileName = Path.Combine(GenshinRegedit.InstallPath, GameFolderName, "YuanShen.exe");

                if (!File.Exists(fileName))
                {
                    fileName = Path.Combine(GenshinRegedit.InstallPath, GameFolderName, "GenshinImpact.exe");
                }

                Process? p = Process.Start(new ProcessStartInfo()
                {
                    UseShellExecute = true,
                    FileName = Path.Combine(GenshinRegedit.InstallPath, GameFolderName, fileName),
                    Arguments = (launchParameter ?? new()).ToString(),
                    WorkingDirectory = Environment.CurrentDirectory,
                    Verb = "runas",
                });
                return p?.MainWindowHandle ?? IntPtr.Zero;
            }
            return IntPtr.Zero;
        }

        internal class LaunchParameter
        {
            public bool? IsFullScreen { get; set; } = null;
            public int? ScreenWidth { get; set; } = null;
            public int? ScreenHeight { get; set; } = null;

            public override string ToString()
            {
                StringBuilder sb = new();

                if (IsFullScreen != null)
                {
                    sb.Append("-screen-fullscreen").Append(' ').Append(IsFullScreen.Value ? 1 : 0);
                }
                if (ScreenWidth != null)
                {
                    sb.Append("-screen-width").Append(' ').Append(ScreenWidth);
                }
                if (ScreenHeight != null)
                {
                    sb.Append("-screen-height").Append(' ').Append(ScreenHeight);
                }
                return sb.ToString();
            }
        }
    }

    internal enum RelaunchMethod
    {
        None,
        Kill,
        Close,
        Logout,
    }
}
