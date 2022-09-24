using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Windows;

namespace GenshinWoodmen.Core
{
    internal static class NoticeService
    {
        static NoticeService()
        {
            ClearNotice();
        }

        public static void AddNotice(string header, string title, string detail = null!, ToastDuration duration = ToastDuration.Short)
        {
            new ToastContentBuilder()
                .AddHeader("AddNotice", header, "AddNotice")
                .AddText(title)
                .AddAttributionTextIf(!string.IsNullOrEmpty(detail), detail)
                .SetToastDuration(duration)
                .ShowSafe();
        }

        public static void AddNoticeWithButton(string header, string title, string button, (string, string) arg, ToastDuration duration = ToastDuration.Short)
        {
            new ToastContentBuilder()
                .AddHeader("AddNotice", header, "AddNotice")
                .AddText(title)
                .AddButton(new ToastButton().SetContent(button).AddArgument(arg.Item1, arg.Item2).SetBackgroundActivation())
                .SetToastDuration(duration)
                .ShowSafe();
        }

        public static void ClearNotice()
        {
            try
            {
                ToastNotificationManagerCompat.History.Clear();
            }
            catch
            {
            }
        }
    }

    internal static class ToastContentBuilderExtensions
    {
        public static ToastContentBuilder AddAttributionTextIf(this ToastContentBuilder builder, bool condition, string text)
        {
            if (condition)
            {
                return builder.AddAttributionText(text);
            }
            else
            {
                return builder.Stub();
            }
        }

        public static ToastContentBuilder Stub(this ToastContentBuilder builder)
        {
            return builder;
        }

        public static void ShowSafe(this ToastContentBuilder builder)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(builder.Show);
            }
            catch (Exception)
            {
            }
        }
    }
}
