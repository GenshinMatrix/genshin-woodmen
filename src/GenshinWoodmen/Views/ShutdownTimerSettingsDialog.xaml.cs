using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows.Input;

namespace GenshinWoodmen.Views;

public partial class ShutdownTimerSettingsDialog : ObservableContentDialog
{
    protected int powerOffAutoHour = 0;
    public int PowerOffAutoHour
    {
        get => powerOffAutoHour;
        set
        {
            Set(ref powerOffAutoHour, value);
            RaisePropertyChanged(nameof(RepresentationalTimeString));
        }
    }

    protected int powerOffAutoMinute = 0;
    public int PowerOffAutoMinute
    {
        get => powerOffAutoMinute;
        set
        {
            Set(ref powerOffAutoMinute, value);
            RaisePropertyChanged(nameof(RepresentationalTimeString));
        }
    }

    public string RepresentationalTimeString => DateTime.Now.AddHours(PowerOffAutoHour).AddMinutes(PowerOffAutoMinute).ToString("yyyy/MM/dd HH:mm:ss");
    public ICommand RepresentationalTimeUpdateCommand { get; }

    public ShutdownTimerSettingsDialog()
    {
        DataContext = this;
        RepresentationalTimeUpdateCommand = new RelayCommand(() =>
        {
            RaisePropertyChanged(nameof(RepresentationalTimeString));
        });
        InitializeComponent();
    }

    public void Setup(int minute)
    {
        PowerOffAutoHour = minute / 60;
        PowerOffAutoMinute = minute % 60;
    }

    public int Setdown()
    {
        return PowerOffAutoHour * 60 + PowerOffAutoMinute;
    }
}
