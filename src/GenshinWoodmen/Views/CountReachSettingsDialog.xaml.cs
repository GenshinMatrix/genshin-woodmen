using GenshinWoodmen.Core;
using GenshinWoodmen.Models;
using ModernWpf.Controls;
using System.IO;
using System.Windows;

namespace GenshinWoodmen.Views
{
    public partial class CountReachSettingsDialog : ContentDialog
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

        public CountReachSettingsDialog(CountSettingsCase @case)
        {
            string whenCountReachedCommandPrev = ((WhenCountReachedCommand ?? string.Empty).Clone() as string)!;

            Case = @case;
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
}
