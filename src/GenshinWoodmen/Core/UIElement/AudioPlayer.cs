namespace GenshinWoodmen.Core
{
    internal static class AudioPlayer
    {
        public static void PlayRepeat(string fileName)
        {
            try
            {
                _ = NativeMethods.MciSendString(@"close temp_music", " ", 0, 0);
                _ = NativeMethods.MciSendString(@"open " + fileName + " alias temp_music", " ", 0, 0);
                _ = NativeMethods.MciSendString(@"play temp_music repeat", " ", 0, 0);
            }
            catch
            {
            }
        }

        public static void Play(string fileName)
        {
            try
            {
                _ = NativeMethods.MciSendString(@"close temp_music", " ", 0, 0);
                _ = NativeMethods.MciSendString(@"open """ + fileName + @""" alias temp_music", null!, 0, 0);
                _ = NativeMethods.MciSendString(@"play temp_music", " ", 0, 0);
            }
            catch
            {
            }
        }

        public static void Stop(string fileName)
        {
            try
            {
                _ = NativeMethods.MciSendString(@"close " + fileName, " ", 0, 0);
                _ = NativeMethods.MciSendString(@"close temp_music", " ", 0, 0);
            }
            catch
            {
            }
        }
    }
}
