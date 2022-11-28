using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace GenshinWoodmen.Core;

public static class StoryboardUtils
{
    public static void BeginBrushStoryboard(DependencyObject dependencyObj, IDictionary<DependencyProperty, Brush> toDictionary, double durationSeconds = 0.2d)
    {
        Storyboard storyboard = new();
        foreach (var keyValue in toDictionary)
        {
            BrushAnimation anima = new()
            {
                To = keyValue.Value,
                Duration = TimeSpan.FromSeconds(durationSeconds),
            };
            Storyboard.SetTarget(anima, dependencyObj);
            Storyboard.SetTargetProperty(anima, new PropertyPath(keyValue.Key));
            storyboard.Children.Add(anima);
        }
        storyboard.Begin();
    }
}
