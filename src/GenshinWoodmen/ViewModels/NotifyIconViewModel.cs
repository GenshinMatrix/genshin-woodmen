using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GenshinWoodmen.Core;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace GenshinWoodmen.ViewModels
{
    public class NotifyIconViewModel : ObservableRecipient
    {
        public static ICommand ShowOrHideCommand => new RelayCommand(() =>
        {
            if (Application.Current.MainWindow.Visibility == Visibility.Visible)
            {
                Application.Current.MainWindow.Hide();
            }
            else
            {
                Application.Current.MainWindow.Activate();
                Application.Current.MainWindow.Focus();
                Application.Current.MainWindow.Show();
            }
        });

        public static ICommand ExitCommand => new RelayCommand(() => Application.Current.Shutdown());

        public static ICommand UsageImageCommand => new RelayCommand(() => _ = UsageManager.ShowUsageImage());
        public static ICommand UsageCommand => new RelayCommand(() => _ = UsageManager.ShowUsage());

        public static ICommand GitHubCommand => new RelayCommand<Window>(async app =>
        {
            try
            {
                _ = Process.Start("explorer.exe", Pack.Url);
            }
            catch
            {
            }
        });
    }
}
