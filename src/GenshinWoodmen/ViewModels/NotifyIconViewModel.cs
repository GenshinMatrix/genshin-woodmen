using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GenshinWoodmen.Core;
using GenshinWoodmen.Models;
using Microsoft.Toolkit.Uwp.Notifications;
using System.Collections.Generic;
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

        public static ICommand RestartCommand => new RelayCommand(() => App.RestartAsElevated());
        public static ICommand ExitCommand => new RelayCommand(() => Application.Current.Shutdown());

        public static ICommand UsageImageCommand => new RelayCommand(() => _ = UsageManager.ShowUsageImage(UsageImageType.Normal));
        public static ICommand UsageImageSingleCommand => new RelayCommand(() => _ = UsageManager.ShowUsageImage(UsageImageType.Single));
        public static ICommand UsageImageMultiCommand => new RelayCommand(() => _ = UsageManager.ShowUsageImage(UsageImageType.Multi));
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

        public static ICommand LaunchGameCommand => new RelayCommand<Window>(async app => _ = await LaunchCtrl.Launch());

        public string Language
        {
            get => Settings.Language.Get();
            set
            {
                if (string.IsNullOrEmpty(value)) return;
                string prev = Language;
                Settings.Language.Set(value);
                Broadcast(prev, value, nameof(Language));
                SettingsManager.Save();
            }
        }
        public static ICommand LanguageZH => new RelayCommand<Window>(async app => SetLanguagePrivate("zh-cn"));
        public static ICommand LanguageJP => new RelayCommand<Window>(async app => SetLanguagePrivate("jp"));
        public static ICommand LanguageEN => new RelayCommand<Window>(async app => SetLanguagePrivate("en-us"));

        private static void SetLanguagePrivate(string name)
        {
            SetLanguage(name);
            Settings.Language.Set(name);
            SettingsManager.Save();
        }

        public static void OnNotificationActivated(ToastNotificationActivatedEventArgsCompat e)
        {
            ToastArguments args = ToastArguments.Parse(e.Argument);

            foreach (KeyValuePair<string, string> arg in args)
            {
                (string k, string v) = arg;

                if (k == "timetoshutdown" && v == "cancel")
                {
                    WeakReferenceMessenger.Default.Send(new CancelShutdownMessage());
                }
            }
        }
    }
}
