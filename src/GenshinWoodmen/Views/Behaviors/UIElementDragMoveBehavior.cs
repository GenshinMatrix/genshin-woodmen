using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;

namespace GenshinWoodmen.Views;

public class UIElementDragMoveBehavior : Behavior<FrameworkElement>
{
    protected override void OnAttached()
    {
        AssociatedObject.MouseLeftButtonDown += MouseLeftButtonDown;
        base.OnAttached();
    }

    protected override void OnDetaching()
    {
        AssociatedObject.MouseLeftButtonDown -= MouseLeftButtonDown;
        base.OnDetaching();
    }

    private void MouseLeftButtonDown(object sender, EventArgs e)
    {
        Window.GetWindow(sender as UIElement)?.DragMove();
    }
}
