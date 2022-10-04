using GenshinWoodmen.Core;
using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Interop;

namespace GenshinWoodmen.Views
{
    public class ToolWindowBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            AssociatedObject.Loaded += Loaded;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= Loaded;
            base.OnDetaching();
        }

        private void Loaded(object sender, EventArgs e)
        {
            NativeMethods.SetToolWindow(new WindowInteropHelper(AssociatedObject).Handle);
        }
    }
}
