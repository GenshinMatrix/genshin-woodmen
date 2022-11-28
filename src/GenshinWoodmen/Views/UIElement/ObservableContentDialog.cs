using ModernWpf.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GenshinWoodmen.Views;

public class ObservableContentDialog : ContentDialog, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new(propertyName));

    protected void Set<T>(ref T field, T newValue, [CallerMemberName] string? propertyName = null)
    {
        field = newValue;
        RaisePropertyChanged(propertyName!);
    }
}
