using CommunityToolkit.Mvvm.Input;
using GenshinWoodmen.Core;

namespace GenshinWoodmen.Views
{
    public partial class DadadaDialog : ObservableContentDialog
    {
        public DadadaDialog()
        {
            InitializeComponent();
            Loaded += async (_, _) => await DadadaManager.Play();
            PrimaryButtonCommand = new RelayCommand(async () => await DadadaManager.Stop());
        }
    }
}
