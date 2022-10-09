using System.Reflection;
using System.Windows;

namespace GenshinWoodmen.Core
{
    internal static class UIElementHelper
    {
        public static FrameworkElement GetTemplateChild(this FrameworkElement d, string childName)
        {
            MethodInfo getTemplateChild = typeof(FrameworkElement).GetMethod("GetTemplateChild", BindingFlags.NonPublic | BindingFlags.Instance)!;
            return (getTemplateChild?.Invoke(d, new object[] { childName }) as FrameworkElement)!;
        }
    }
}
