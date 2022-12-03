using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading.Tasks;

namespace GenshinWoodmen.Core;

internal sealed class CompatibleBilibili
{
    public static bool IsAvailabled { get; private set; } = false;

    public static bool RefreshAvailabled()
    {
        try
        {
            string configIni = @$"{GenshinRegedit.InstallPath}\config.ini";
            string[] lines = File.ReadAllLines(configIni);

            foreach (string line in lines)
            {
                string kv = line.Trim();
                if (kv.StartsWith("cps="))
                {
                    if (kv.EndsWith("bilibili"))
                    {
                        return IsAvailabled = true;
                    }
                    break;
                }
            }
        }
        catch
        {
        }
        return IsAvailabled = false;
    }

    public static async Task<bool> Login()
    {
        return await Task.Run(async () =>
        {
            if (Process.GetProcessesByName("YuanShen").FirstOrDefault() is Process p)
            {
                IntPtr hwndLogin = IntPtr.Zero;

                _ = NativeMethods.EnumWindows((IntPtr hwnd, int lParam) =>
                {
                    try
                    {
                        _ = NativeMethods.GetWindowThreadProcessId(hwnd, out int pid);

                        if (pid == p.Id)
                        {
                            string title = NativeMethods.GetWindowTitle(hwnd);

                            if (!string.IsNullOrEmpty(title))
                            {
                                Logger.Info($"[BilibiliServer] {title}");
                                hwndLogin = hwnd;
                                return false;
                            }
                        }
                    }
                    catch
                    {
                    }
                    return true;
                }, 0);
                if (hwndLogin == IntPtr.Zero) return false;
                NativeMethods.Focus(hwndLogin);
                NativeMethods.CursorCenterPos(hwndLogin, offsetY: 110);
                UserSimulator.Input.Mouse.LeftButtonClick();
                return true;
            }
            return false;
        });
    }

    [Conditional("DEBUG")]
    public static void DebugSearch()
    {
        using ManagementObjectSearcher searcher = new("SELECT ProcessId, ExecutablePath, CommandLine FROM Win32_Process");
        using ManagementObjectCollection results = searcher.Get();
        var query = from ps in Process.GetProcesses()
                    join mo in results.Cast<ManagementObject>()
                    on ps.Id equals (int)(uint)mo["ProcessId"]
                    select new
                    {
                        Process = ps,
                        Path = (string)mo["ExecutablePath"],
                        CommandLine = (string)mo["CommandLine"],
                    };
        foreach (var item in query)
        {
            Logger.Info($"[{item.Process.Id}] {item.Path}|{item.CommandLine}");
        }
    }
}
