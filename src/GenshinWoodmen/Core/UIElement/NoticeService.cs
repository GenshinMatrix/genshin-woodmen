using Microsoft.Toolkit.Uwp.Notifications;

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
                .Show();
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
    }
}
