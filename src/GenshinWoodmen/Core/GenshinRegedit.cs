using Microsoft.Win32;
using System;
using System.Diagnostics;

namespace GenshinWoodmen.Core
{
    internal class GenshinRegedit
    {
        public static string InstallPath
        {
            get
            {
                try
                {
                    using RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                    RegistryKey? key = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\原神");

                    if (key == null)
                    {
                        key = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Genshin Impact");

                        if (key == null)
                        {
                            return null!;
                        }
                    }

                    object installLocation = key.GetValue("InstallPath")!;
                    key?.Dispose();

                    if (installLocation != null && !string.IsNullOrEmpty(installLocation.ToString()))
                    {
                        return installLocation.ToString()!;
                    }
                }
                catch (Exception e)
                {
                    NoticeService.AddNotice(Mui("Tips"), "Failed", e.Message);
                }
                return null!;
            }
        }
    }
}
