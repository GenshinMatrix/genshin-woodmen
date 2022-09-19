using System.Reflection;
using System.Windows;

namespace GenshinWoodmen.Core
{
    internal static class ElementHelper
    {
        public static FrameworkElement? GetTemplateChild(this FrameworkElement d, string childName)
        {
            MethodInfo getTemplateChild = typeof(FrameworkElement).GetMethod("GetTemplateChild", BindingFlags.NonPublic | BindingFlags.Instance)!;

            return getTemplateChild?.Invoke(d, new object[] { childName }) as FrameworkElement;
        }

        public static FrameworkElement? GetVisualChild(this FrameworkElement d, int index)
        {
            MethodInfo getVisualChild = typeof(FrameworkElement).GetMethod("GetVisualChild", BindingFlags.NonPublic | BindingFlags.Instance)!;

            return getVisualChild?.Invoke(d, new object[] { index }) as FrameworkElement;
        }

    }
}
