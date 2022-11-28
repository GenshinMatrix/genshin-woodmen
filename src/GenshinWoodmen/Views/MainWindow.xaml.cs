using GenshinWoodmen.ViewModels;
using System.Windows;

namespace GenshinWoodmen.Views;

public partial class MainWindow : Window
{
    public MainViewModel ViewModel { get; }

    public MainWindow()
    {
        DataContext = ViewModel = new(this);
        InitializeComponent();
    }
}
