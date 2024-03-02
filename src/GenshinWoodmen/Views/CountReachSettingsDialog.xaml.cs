using CommunityToolkit.Mvvm.Input;
using GenshinWoodmen.Core;
using GenshinWoodmen.Models;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace GenshinWoodmen.Views;

public partial class CountReachSettingsDialog : ObservableContentDialog
{
    public CountSettingsCase Case
    {
        get => (CountSettingsCase)GetValue(CaseProperty);
        set => SetValue(CaseProperty, value);
    }

    public static readonly DependencyProperty CaseProperty = DependencyProperty.Register("Case", typeof(CountSettingsCase), typeof(CountReachSettingsDialog), new(CountSettingsCase.Notification));

    public string WhenCountReachedCommand
    {
        get => Settings.WhenCountReachedCommand.Get();
        set => Settings.WhenCountReachedCommand.Set(value);
    }

    private bool isCountStartedFromNextUpdateTime = UpdateTime.IsCountStartedFormNextUpdateTime;

    public bool IsCountStartedFromNextUpdateTime
    {
        get => isCountStartedFromNextUpdateTime;
        set
        {
            UpdateTime.IsCountStartedFormNextUpdateTime = value;
            Set(ref isCountStartedFromNextUpdateTime, value);
        }
    }

    public string NextUpdateTimeViewString => UpdateTime.NextUpdateTimeViewString;

    public ICommand UpdateNextUpdateTimeCommand => new RelayCommand(() =>
    {
        _ = UpdateTime.UpdateNextTime();
        RaisePropertyChanged(nameof(NextUpdateTimeViewString));
    });

    public CountReachSettingsDialog()
    {
        string whenCountReachedCommandPrev = ((WhenCountReachedCommand ?? string.Empty).Clone() as string)!;

        DataContext = this;
        InitializeComponent();
        PrimaryButtonClick += (_, _) =>
        {
            if (!whenCountReachedCommandPrev.Equals(WhenCountReachedCommand))
            {
                if (File.Exists(WhenCountReachedCommand))
                    WhenCountReachedCommand = $"\"{WhenCountReachedCommand}\"";
                SettingsManager.Save();
            }
        };
    }
}
