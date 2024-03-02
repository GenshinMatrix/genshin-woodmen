using CommunityToolkit.Mvvm.Input;
using GenshinWoodmen.Core;
using GenshinWoodmen.Models;
using System.Globalization;
using System.Windows;

namespace GenshinWoodmen.Views;

public partial class DadadaDialog : ObservableContentDialog
{
    public Visibility DefaultCoverVisibility => new CultureInfo(Settings.Language.Get() ?? "zh").TwoLetterISOLanguageName.ToLower() switch
    {
        "zh" or "jp" => Visibility.Visible, // for kanji only
        _ => Visibility.Collapsed,
    };

    public Visibility DefaultCoverVisibilityInverted => DefaultCoverVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

    public DadadaDialog()
    {
        DataContext = this;
        InitializeComponent();
        Loaded += async (_, _) => await DadadaManager.Play();
        PrimaryButtonCommand = new RelayCommand(async () => await DadadaManager.Stop());
    }
}
