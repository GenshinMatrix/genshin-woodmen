using GenshinWoodmen.Core;
using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace GenshinWoodmen
{
    public partial class App : Application
    {
        public static new App? Current { get; protected set; } = null!;
        public TaskbarIcon Taskbar { get; protected set; } = null!;
        public static bool IsElevated { get; } = GetElevated();

        public App()
        {
            Logger.Info("Startup");
            Current = this;
            Current.DispatcherUnhandledException += (_, e) => e.Handled = true;
            AppDomain.CurrentDomain.UnhandledException += (s, e) => _ = e;
            NoticeService.ClearNotice();
            EnsureElevated();
            CheckSingleInstance();
            InitializeComponent();
            SetupLanguage();
            SettingsManager.Setup();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Taskbar = (TaskbarIcon)FindResource("PART_Taskbar");
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            UsageManager.CleanUsageImage();
            NoticeService.ClearNotice();
            base.OnExit(e);
        }

        public void EnsureElevated()
        {
            if (!IsElevated)
            {
                RestartAsElevated('r' + 'u' + 'n' + 'a' + 's');
            }
        }

        public static void RestartAsElevated(int? exitCode = null)
        {
            try
            {
                _ = Process.Start(new ProcessStartInfo()
                {
                    Verb = "runas",
                    UseShellExecute = true,
                    FileName = Process.GetCurrentProcess().MainModule!.FileName,
                    WorkingDirectory = Environment.CurrentDirectory,
                });
            }
            catch (Win32Exception)
            {
                return;
            }
            Current!.Shutdown();
            if (exitCode != null) Environment.Exit(exitCode.Value);
        }

        private static bool GetElevated()
        {
            using WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public void CheckSingleInstance()
        {
            EventWaitHandle? handle;

            try
            {
                handle = EventWaitHandle.OpenExisting(Pack.Name);
                handle.Set();
                Shutdown();
            }
            catch (WaitHandleCannotBeOpenedException)
            {
                handle = new EventWaitHandle(false, EventResetMode.AutoReset, Pack.Name);
            }
            GC.KeepAlive(handle);
            _ = Task.Run(() =>
            {
                while (handle.WaitOne())
                {
                    Dispatcher.Invoke(() =>
                    {
                        MainWindow?.Activate();
                        MainWindow?.Focus();
                        MainWindow?.Show();
                    });
                }
            });
        }
    }
}
