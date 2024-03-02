using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GenshinWoodmen.Core;
using GenshinWoodmen.Models;
using GenshinWoodmen.Views;
using Microsoft.Toolkit.Uwp.Notifications;
using ModernWpf.Controls;
using SharpVectors.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GenshinWoodmen.ViewModels;

public class MainViewModel : ObservableRecipient, IRecipient<CountMessage>, IRecipient<StatusMessage>, IRecipient<CancelShutdownMessage>
{
    public MainWindow Source { get; set; } = null!;
    public double Left => SystemParameters.WorkArea.Right;
    public double Top => SystemParameters.WorkArea.Bottom;

    public int ForecastX3 => (int)(((Settings.DelayLogin / 1000d) + (Settings.DelayLaunch / 1000d) + 7d) * (2000d / 3d) / 60d);
    public int ForecastX6 => (int)Math.Floor(ForecastX3 / 2d);
    public int ForecastX9 => (int)Math.Floor(ForecastX3 / 3d);
    public int ForecastX12 => (int)Math.Floor(ForecastX3 / 4d);
    public int ForecastX15 => (int)Math.Floor(ForecastX3 / 5d);
    public int ForecastX18 => (int)Math.Floor(ForecastX3 / 6d);
    public int ForecastX21 => (int)Math.Floor(ForecastX3 / 7d);
    public int ForecastX30 => (int)Math.Floor(ForecastX3 / 10d);

    public int ForecastX3Count => (int)Math.Floor(2000d / 3d);
    public int ForecastX6Count => (int)Math.Floor(2000d / 6d);
    public int ForecastX9Count => (int)Math.Floor(2000d / 9d);
    public int ForecastX12Count => (int)Math.Floor(2000d / 12d);
    public int ForecastX15Count => (int)Math.Floor(2000d / 15d);
    public int ForecastX18Count => (int)Math.Floor(2000d / 18d);
    public int ForecastX21Count => (int)Math.Floor(2000d / 21d);
    public int ForecastX30Count => (int)Math.Floor(2000d / 30d);

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

    public AutoMuteSelection AutoMute
    {
        get => (AutoMuteSelection)Settings.AutoMute.Get();
        set
        {
            if (value == AutoMuteSelection.ConverterIgnore) return;
            AutoMuteSelection prev = AutoMute;
            Broadcast(prev, value, nameof(AutoMute));
            Settings.AutoMute.Set((int)value);
            SettingsManager.Save();
        }
    }

    protected byte brightness = NativeMethods.GetBrightness();

    public byte Brightness
    {
        get => brightness;
        set => SetProperty(ref brightness, value);
    }

    public ICommand StartCommand => new RelayCommand<Button>(async button =>
    {
        TextBlock startIcon = (Window.GetWindow(button).FindName("TextBlockStartIcon") as TextBlock)!;
        TextBlock start = (Window.GetWindow(button).FindName("TextBlockStart") as TextBlock)!;
        SvgViewbox mainIcon = (Window.GetWindow(button).FindName("SvgViewBoxMainIcon") as SvgViewbox)!;

        Brush brush = null!;
        button!.IsEnabled = false;
        if (startIcon.Text.Equals(FluentSymbol.Start))
        {
            brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EEDBE8F6"));
            start.Text = Mui("ButtonStop");
            startIcon.Text = FluentSymbol.Stop;
            mainIcon.SetColor("Green");
            JiggingProcessor.Start();
        }
        else
        {
            brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EEFFFFFF"));
            start.Text = Mui("ButtonStart");
            startIcon.Text = FluentSymbol.Start;
            mainIcon.SetColor("Black");
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

    public ICommand ConfigOpenWithNotepadCommand => new RelayCommand(async () =>
    {
        try
        {
            _ = Process.Start(new ProcessStartInfo()
            {
                FileName = "notepad.exe",
                Arguments = $"\"{SettingsManager.Path}\"",
                UseShellExecute = false,
                CreateNoWindow = true,
            });
        }
        catch
        {
        }
    });

    public ICommand ConfigOpenWithCommand => new RelayCommand(async () =>
    {
        try
        {
            _ = Process.Start(new ProcessStartInfo()
            {
                FileName = "openwith.exe",
                Arguments = $"\"{SettingsManager.Path}\"",
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
        Broadcast(ForecastX18, ForecastX18, nameof(ForecastX18));
        Broadcast(ForecastX21, ForecastX21, nameof(ForecastX21));
        Broadcast(ForecastX30, ForecastX30, nameof(ForecastX30));
        SetupLanguage();
    });

    public ICommand ClearCountCommand => new RelayCommand(() => MaxCount = CurrentCount = 0);

    public ICommand TopMostCommand => new RelayCommand<Window>(async app =>
    {
        app!.Topmost = !app.Topmost;
        if (app.FindName("TextBlockTopMost") is TextBlock topMostIcon)
        {
            topMostIcon.Text = app.Topmost ? FluentSymbol.Unpin : FluentSymbol.Pin;
        }
    });

    public ICommand RestorePosCommand => new RelayCommand<Window>(async app =>
    {
        app!.Left = Left - app.Width;
        app!.Top = Top - app.Height;
    });

    public ICommand RestartCommand => NotifyIconViewModel.RestartCommand;
    public ICommand ExitCommand => NotifyIconViewModel.ExitCommand;
    public ICommand UsageCommand => NotifyIconViewModel.UsageCommand;
    public ICommand UsageImageCommand => NotifyIconViewModel.UsageImageCommand;
    public ICommand UsageImageSingleCommand => NotifyIconViewModel.UsageImageSingleCommand;
    public ICommand UsageImageMultiCommand => NotifyIconViewModel.UsageImageMultiCommand;
    public ICommand GitHubCommand => NotifyIconViewModel.GitHubCommand;

    public ICommand MuteGameCommand => new RelayCommand(async () => await MuteManager.MuteGameAsync(true));
    public ICommand UnmuteGameCommand => new RelayCommand(async () => await MuteManager.MuteGameAsync(false));
    public ICommand MuteSystemCommand => new RelayCommand(() => MuteManager.MuteSystem(true));
    public ICommand UnmuteSystemCommand => new RelayCommand(() => MuteManager.MuteSystem(false));

    protected CountSettingsCase countSettingsCase;

    public CountSettingsCase CountSettingsCase
    {
        get => countSettingsCase;
        set
        {
            if (value == CountSettingsCase.ConverterIgnore) return;
            SetProperty(ref countSettingsCase, value);
        }
    }

    protected bool CountSettingsDialogShown = false;

    public ICommand CountSettingsCommand => new RelayCommand(async () =>
    {
        if (CountSettingsDialogShown) return;
        using DialogWindow win = new()
        {
            Width = SystemParameters.WorkArea.Width,
            Height = SystemParameters.WorkArea.Height,
        };
        win.Show();
        CountReachSettingsDialog dialog = new()
        {
            Case = CountSettingsCase,
        };
        CountSettingsDialogShown = true;
        await dialog.ShowAsync(ContentDialogPlacement.Popup);
        CountSettingsDialogShown = false;
        CountSettingsCase = dialog.Case;
    });

    protected bool ShutdownTimerSettingsDialogShown = false;

    public ICommand ShutdownTimerSettingsCommand => new RelayCommand(async () =>
    {
        if (ShutdownTimerSettingsDialogShown) return;
        using DialogWindow win = new()
        {
            Width = SystemParameters.WorkArea.Width,
            Height = SystemParameters.WorkArea.Height,
        };
        win.Show();
        ShutdownTimerSettingsDialog dialog = new();
        ShutdownTimerSettingsDialogShown = true;
        dialog.Setup(PowerOffAutoMinute);
        await dialog.ShowAsync(ContentDialogPlacement.Popup);
        if (!PowerOffAuto) PowerOffAutoMinute = dialog.Setdown();
        ShutdownTimerSettingsDialogShown = false;
    });

    public bool TwiceEsc
    {
        get => LaunchCtrl.TwiceEsc;
        set => LaunchCtrl.TwiceEsc = value;
    }

    public bool TwiceEscReplacedLeftClick
    {
        get => LaunchCtrl.TwiceEscReplacedLeftClick;
        set => LaunchCtrl.TwiceEscReplacedLeftClick = value;
    }

    public MainViewModel(MainWindow source)
    {
        Source = source;

        _ = UpdateTime.UpdateNextTime();

        Source.Loaded += (_, _) =>
        {
            Source.BrightnessMenuItem.Loaded += (_, _) =>
            {
                if (Source.BrightnessMenuItem.GetTemplateChild("BrightnessSlider") is Slider brightnessSlider)
                {
                    brightnessSlider.IsVisibleChanged += (_, _) =>
                    {
                        Brightness = NativeMethods.GetBrightness();
                    };
                    brightnessSlider.ValueChanged += (_, _) =>
                    {
                        if (brightnessSlider.IsVisible)
                        {
                            NativeMethods.SetBrightness(Brightness);
                        }
                    };
                }
            };
        };

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
                    OnCountReached();
                }
            }
            if (PowerOffAuto && PowerOffDateTime != null && powerOffAutoMinuteByUser > 0)
            {
                TimeSpan timeOffset = (PowerOffDateTime - DateTime.Now).Value;

                if (!PowerOffNotified && (PowerOffAutoMinute = (int)timeOffset.TotalMinutes) <= 3)
                {
                    NoticeService.AddNoticeWithButton(Mui("Tips"), string.Format(Mui("PowerOffTips1"), Math.Round(timeOffset.TotalMinutes)), Mui("Cancel"), ("timetoshutdown", "cancel"), ToastDuration.Long);
                    PowerOffNotified = true;
                }
                if (timeOffset.TotalSeconds <= 0)
                {
                    switch ((AutoMuteSelection)Settings.AutoMute.Get())
                    {
                        case AutoMuteSelection.AutoMuteSystem:
                        case AutoMuteSelection.AutoMuteGame:
                            _ = JiggingProcessor.SetMute(false); // Restored mute -> false
                            break;
                    }

                    NoticeService.ClearNotice();
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

    private async void OnCountReached()
    {
        switch (CountSettingsCase)
        {
            case CountSettingsCase.Notification:
                NoticeService.AddNotice(Mui("Tips"), string.Format(Mui("CountReachStop"), MaxCount), string.Empty, ToastDuration.Short);
                break;

            case CountSettingsCase.CloseGame:
                await LaunchCtrl.Close();
                break;

            case CountSettingsCase.Shutdown:
                switch ((AutoMuteSelection)Settings.AutoMute.Get())
                {
                    case AutoMuteSelection.AutoMuteSystem:
                    case AutoMuteSelection.AutoMuteGame:
                        await Task.Delay(200); // Chatting for mute process
                                               // TODO: Try to use NAudio lib to mute for fixing it
                        await JiggingProcessor.SetMute(true); // Restored mute -> true
                        break;
                }
                await Source.Dispatcher.BeginInvoke(() =>
                {
                    powerOffAutoMinuteByUser = PowerOffAutoMinute = 3;
                    PowerOffDateTime = DateTime.Now.AddMinutes(PowerOffAutoMinute);
                    PowerOffAuto = true;
                });
                break;

            case CountSettingsCase.Dadada:
                await DadadaManager.Show();
                break;

            case CountSettingsCase.Customize:
                await Task.Run(() =>
                {
                    if (string.IsNullOrEmpty(Settings.WhenCountReachedCommand))
                    {
                        NoticeService.AddNotice(Mui("Tips"), string.Format(Mui("CountReachStop"), MaxCount), string.Empty, ToastDuration.Short);
                        return;
                    }
                    try
                    {
                        _ = Process.Start(new ProcessStartInfo()
                        {
                            FileName = "cmd.exe",
                            Arguments = $"/c {Settings.WhenCountReachedCommand.Get()}",
                            UseShellExecute = false,
                            CreateNoWindow = true,
                        });
                    }
                    catch
                    {
                    }
                });
                break;
        }
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

    void IRecipient<CountMessage>.Receive(CountMessage message)
    {
        if (UpdateTime.IsCountStartedFormNextUpdateTime)
        {
            if ((DateTime.Now - UpdateTime.NextUpdateTime).TotalSeconds >= 0d)
            {
                CurrentCount++;
            }
        }
        else
        {
            CurrentCount++;
        }
    }

    void IRecipient<StatusMessage>.Receive(StatusMessage message) => CurrentStatus = message.ToString();

    void IRecipient<CancelShutdownMessage>.Receive(CancelShutdownMessage message)
    {
        PowerOffAuto = false;
        NoticeService.ClearNotice();
        switch ((AutoMuteSelection)Settings.AutoMute.Get())
        {
            case AutoMuteSelection.AutoMuteSystem:
            case AutoMuteSelection.AutoMuteGame:
                _ = JiggingProcessor.SetMute(false); // Restored mute -> false
                break;
        }
    }
}
