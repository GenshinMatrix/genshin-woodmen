using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;

namespace GenshinWoodmen.Views
{
    public class UIElementDragMoveBehavior : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            AssociatedObject.MouseLeftButtonDown += MouseDown;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.MouseLeftButtonDown -= MouseDown;
            base.OnDetaching();
        }

        private void MouseDown(object sender, EventArgs ea)
        {
            Window.GetWindow(sender as UIElement)?.DragMove();
        }
    }
}
