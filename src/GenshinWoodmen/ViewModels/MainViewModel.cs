using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GenshinWoodmen.Core;
using GenshinWoodmen.Models;
using GenshinWoodmen.Views;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Keys = System.Windows.Forms.Keys;

namespace GenshinWoodmen.ViewModels
{
    public class MainViewModel : ObservableRecipient, IRecipient<CountMessage>, IRecipient<StatusMessage>
    {
        public MainWindow Source { get; set; } = null!;
        public double Left => SystemParameters.WorkArea.Right;
        public double Top => SystemParameters.WorkArea.Bottom;

        public int ForecastX3 => (int)(((Settings.DelayLogin / 1000d) + (Settings.DelayLaunch / 1000d) + 10d) * 2000d / 60d);
        public int ForecastX6 => ForecastX3 / 2;
        public int ForecastX9 => ForecastX3 / 3;
        public int ForecastX12 => ForecastX3 / 4;
        public int ForecastX15 => ForecastX3 / 5;

        protected int currentCount = 0;
        public int CurrentCount
        {
            get => currentCount;
            internal set => SetProperty(ref currentCount, value);
        }

        protected int maxCount = 0;
        public int MaxCount
        {
            get => maxCount;
            set => SetProperty(ref maxCount, value);
        }

        protected string currentStatus = null!;
        public string CurrentStatus
        {
            get => currentStatus;
            internal set => SetProperty(ref currentStatus, value);
        }

        protected bool powerOffAuto = false;
        public bool PowerOffAuto
        {
            get => powerOffAuto;
            set => SetProperty(ref powerOffAuto, value);
        }

        protected int powerOffAutoMinuteByUser = 0;
        protected int powerOffAutoMinute = 0;
        public int PowerOffAutoMinute
        {
            get => powerOffAutoMinute;
            set => SetProperty(ref powerOffAutoMinute, value);
        }

        public DateTime? PowerOffDateTime { get; protected set; } = null!;
        public bool PowerOffNotified { get; protected set; } = false;

        public ICommand StartCommand => new RelayCommand<Button>(async button =>
        {
            TextBlock startIcon = (Window.GetWindow(button).FindName("TextBlockStartIcon") as TextBlock)!;
            TextBlock start = (Window.GetWindow(button).FindName("TextBlockStart") as TextBlock)!;

            Brush brush = null!;
            button!.IsEnabled = false;
            if (startIcon.Text.Equals(FluentSymbol.Start))
            {
                brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EEDBE8F6"));
                start.Text = Mui("ButtonStop");
                startIcon.Text = FluentSymbol.Stop;
                JiggingProcessor.Start();
            }
            else
            {
                brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EEFFFFFF"));
                start.Text = Mui("ButtonStart");
                startIcon.Text = FluentSymbol.Start;
                JiggingProcessor.Stop();
            }

            Border border = (Window.GetWindow(button).FindName("Border") as Border)!;
            StoryboardUtils.BeginBrushStoryboard(border, new Dictionary<DependencyProperty, Brush>()
            {
                [Border.BackgroundProperty] = brush,
            });
            await Task.Delay(1000);
            button!.IsEnabled = true;
        });

        public ICommand ConfigOpenCommand => new RelayCommand(async () =>
        {
            try
            {
                _ = Process.Start(new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c \"{SettingsManager.Path}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                });
            }
            catch
            {
            }
        });

        public ICommand ConfigReloadCommand => new RelayCommand(async () =>
        {
            SettingsManager.Reinit();
            Broadcast(ForecastX3, ForecastX3, nameof(ForecastX3));
            Broadcast(ForecastX6, ForecastX6, nameof(ForecastX6));
            Broadcast(ForecastX9, ForecastX9, nameof(ForecastX9));
            Broadcast(ForecastX12, ForecastX12, nameof(ForecastX12));
            Broadcast(ForecastX15, ForecastX15, nameof(ForecastX15));
            SetupLanguage();
        });

        public ICommand ClearCountCommand => new RelayCommand(() => CurrentCount = 0);
        public ICommand TopMostCommand => new RelayCommand<Window>(async app =>
        {
            app!.Topmost = !app.Topmost;
            if (app.FindName("TextBlockTopMost") is TextBlock topMostIcon)
            {
                topMostIcon.Text = app.Topmost ? FluentSymbol.Unpin : FluentSymbol.Pin;
            }
        });
        public ICommand ExitCommand => NotifyIconViewModel.ExitCommand;
        public ICommand UsageCommand => NotifyIconViewModel.UsageCommand;
        public ICommand UsageImageCommand => NotifyIconViewModel.UsageImageCommand;

        public MainViewModel(MainWindow source)
        {
            Source = source;
            WeakReferenceMessenger.Default.RegisterAll(this);
            GC.KeepAlive(new PeriodProcessor(() =>
            {
                if (CurrentCount > 0 && MaxCount > 0 && CurrentCount >= MaxCount)
                {
                    if (!JiggingProcessor.IsCanceled)
                    {
                        Source.Dispatcher.Invoke(() =>
                        {
                            StartCommand?.Execute(Source.FindName("ButtonStart"));
                        });
                        NoticeService.AddNotice(Mui("Tips"), string.Format(Mui("CountReachStop"), MaxCount), string.Empty, ToastDuration.Short);
                    }
                }
                if (PowerOffAuto && PowerOffDateTime != null && powerOffAutoMinuteByUser > 0)
                {
                    TimeSpan timeOffset = (PowerOffDateTime - DateTime.Now).Value;

                    if (!PowerOffNotified && (PowerOffAutoMinute = (int)timeOffset.TotalMinutes) <= 3)
                    {
                        NoticeService.AddNotice(Mui("Tips"), string.Format(Mui("PowerOffTips1"), Math.Round(timeOffset.TotalMinutes)), Mui("PowerOffTips2"), ToastDuration.Long);
                        PowerOffNotified = true;
                    }
                    if (timeOffset.TotalSeconds <= 0)
                    {
                        NativeMethods.ShutdownPowerOff();
                    }
                }
            }));

            PropertyChanged += (_, e) =>
            {
                switch (e.PropertyName)
                {
                    case nameof(PowerOffAuto):
                        if (PowerOffAuto)
                        {
                            if (PowerOffAutoMinute == 0)
                            {
                                PowerOffAuto = false;
                                return;
                            }
                            PowerOffDateTime = DateTime.Now.AddMinutes(PowerOffAutoMinute);
                            powerOffAutoMinuteByUser = PowerOffAutoMinute;
                        }
                        else
                        {
                            PowerOffDateTime = null;
                        }
                        PowerOffNotified = false;
                        break;
                }
            };

            SettingsManager.Reloaded += RegisterHotKey;
            RegisterHotKey();
        }

        private void RegisterHotKey()
        {
            try
            {
                HotkeyHolder.RegisterHotKey(Settings.ShortcutKey, (s, e) =>
                {
                    StartCommand?.Execute(Source.FindName("ButtonStart"));
                });
            }
            catch (Exception e)
            {
                Logger.Exception(e);
            }
        }

        void IRecipient<CountMessage>.Receive(CountMessage message) => CurrentCount++;
        void IRecipient<StatusMessage>.Receive(StatusMessage message) => CurrentStatus = message.ToString();
    }
}
