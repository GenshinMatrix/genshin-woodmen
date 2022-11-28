using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace GenshinWoodmen.Views;

public class LeftContextMenuBehavior : Behavior<FrameworkElement>
{
    public Point? PlacementOffset { get; set; } = null;
    public PlacementMode Placement { get; set; } = PlacementMode.Bottom;

    public double? PlacementOffsetX
    {
        get => PlacementOffset?.X;
        set => PlacementOffset = value != null ? new(value ?? 0d, PlacementOffset?.Y ?? 0d) : null;
    }

    public double? PlacementOffsetY
    {
        get => PlacementOffset?.Y;
        set => PlacementOffset = value != null ? new(PlacementOffset?.X ?? 0d, value ?? 0d) : null;
    }

    public LeftContextMenuBehavior()
    {
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        Register(AssociatedObject, PlacementOffset, Placement);
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
    }

    private void Register(FrameworkElement frameworkElement, Point? placementOffset = null, PlacementMode placement = PlacementMode.Bottom)
    {
        if (frameworkElement?.ContextMenu == null)
        {
            return;
        }
        frameworkElement.PreviewMouseRightButtonUp += (s, e) => e.Handled = true;
        frameworkElement.MouseRightButtonUp += (s, e) => e.Handled = true;
        frameworkElement.PreviewMouseLeftButtonDown += (s, e) =>
        {
            ContextMenu contextMenu = frameworkElement.ContextMenu;

            if (contextMenu != null)
            {
                if (contextMenu.PlacementTarget != frameworkElement)
                {
                    contextMenu.PlacementTarget = frameworkElement;
                    contextMenu.PlacementRectangle = new Rect(placementOffset ?? new Point(), new Size(frameworkElement.ActualWidth, frameworkElement.ActualHeight));
                    contextMenu.Placement = placement;
                    contextMenu.StaysOpen = false;
                }
                contextMenu.IsOpen = !contextMenu.IsOpen;
            }
        };
    }
}
