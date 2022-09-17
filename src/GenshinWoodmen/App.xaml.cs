using GenshinWoodmen.Core;
using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace GenshinWoodmen
{
    public partial class App : Application
    {
        public static new App? Current { get; protected set; } = null!;
        public TaskbarIcon Taskbar { get; protected set; } = null!;

        public App()
        {
            Logger.Info("Startup");
            Current = this;
            NoticeService.ClearNotice();
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
