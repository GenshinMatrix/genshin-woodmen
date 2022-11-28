using ModernWpf.Controls;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace GenshinWoodmen.Views;

public partial class DialogWindow : Window, IDisposable
{
    public DialogWindow()
    {
        InitializeComponent();
    }

    public void Dispose()
    {
        Close();
    }

    public static async Task ShowMessage(string title, string message)
    {
        using DialogWindow win = new()
        {
            Width = SystemParameters.WorkArea.Width,
            Height = SystemParameters.WorkArea.Height,
        };
        win.Show();
        MessageDialog dialog = new(title, message);
        await dialog.ShowAsync(ContentDialogPlacement.Popup);
    }
}
