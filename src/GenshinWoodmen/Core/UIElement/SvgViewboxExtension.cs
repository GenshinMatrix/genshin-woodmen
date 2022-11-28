using SharpVectors.Converters;
using System.Windows.Media;

namespace GenshinWoodmen.Core;

internal static class SvgViewboxExtension
{
    public static void SetColor(this SvgViewbox control, string color)
    {
        static void ChangeDrawingGroupColor(DrawingGroup dg, Brush brush)
        {
            foreach (Drawing d in dg.Children) ChangeDrawingColor(d, brush);
        }

        static void ChangeDrawingColor(Drawing d, Brush brush)
        {
            if (d is DrawingGroup dg) ChangeDrawingGroupColor(dg, brush);
            else if (d is GeometryDrawing g)
            {
                g.Brush = brush;
                if (g.Pen != null) g.Pen.Brush = brush;
            }
        }

        static void ChangeSvgViewboxColor(SvgViewbox control, Brush brush)
        {
            foreach (Drawing d in control.Drawings.Children) ChangeDrawingColor(d, brush);
        }

        control.Dispatcher.Invoke(() => ChangeSvgViewboxColor(control, (new BrushConverter().ConvertFromString(color) as Brush)!));
    }
}
