using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GenshinWoodmen.Views;

/// <summary>
/// https://stackoverflow.com/questions/35165349/how-to-drag-rendertransform-with-mouse-in-wpf
/// </summary>
public class PanAndZoomCanvas : Canvas
{
    public MatrixTransform Transform { get; set; } = new();
    public float? MinDeterminant { get; set; } = null!;
    public float? MaxDeterminant { get; set; } = null!;
    public float Zoomfactor { get; set; } = 1.333f;
    protected Point MousePosition = new();
    protected bool DownFlow = false;

    public PanAndZoomCanvas()
    {
        MouseDown += OnMouseDown;
        MouseUp += OnMouseUp;
        MouseMove += OnMouseMove;
        MouseWheel += OnMouseWheel;
    }

    private void OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left && e.ClickCount == 1)
        {
            MousePosition = Transform.Inverse.Transform(e.GetPosition(this));
            DownFlow = true;
        }
    }

    private void OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        DownFlow = false;
    }

    private void OnMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed && DownFlow)
        {
            Point mousePosition = Transform.Inverse.Transform(e.GetPosition(this));
            Vector delta = Point.Subtract(mousePosition, MousePosition);
            var translate = new TranslateTransform(delta.X, delta.Y);
            Transform.Matrix = translate.Value * Transform.Matrix;

            foreach (UIElement child in Children)
            {
                child.RenderTransform = Transform;
            }
        }
    }

    private void OnMouseWheel(object sender, MouseWheelEventArgs e)
    {
        float scaleFactor = Zoomfactor;
        if (e.Delta < 0)
        {
            scaleFactor = 1f / scaleFactor;
        }

        Point mousePostion = e.GetPosition(this);

        Matrix scaleMatrix = Transform.Matrix;
        scaleMatrix.ScaleAt(scaleFactor, scaleFactor, mousePostion.X, mousePostion.Y);
        if (scaleMatrix.Determinant <= MinDeterminant || scaleMatrix.Determinant >= MaxDeterminant) return;
        Transform.Matrix = scaleMatrix;

        SetTransform(Transform);
    }

    public void SetTransform(MatrixTransform transform)
    {
        foreach (UIElement child in Children)
        {
            double x = Canvas.GetLeft(child);
            double y = Canvas.GetTop(child);

            double sx = x;
            double sy = y;

            Canvas.SetLeft(child, sx);
            Canvas.SetTop(child, sy);

            child.RenderTransform = Transform = transform;
        }
    }
}
