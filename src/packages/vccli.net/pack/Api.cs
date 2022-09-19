using System.Diagnostics;
using System.Threading.Tasks;

namespace vccli.net
{
    public static class Api
    {
        public static void Mute(int pid)
        {
            Call($"{pid} --mute");
        }

        public static void Unmute(int pid)
        {
            Call($"{pid} --unmute");
        }

        private static void Call(string Arguments)
        {
            Process? p = Process.Start(new ProcessStartInfo("vccli.exe")
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                Arguments = Arguments
            });
            p?.WaitForExit();
        }

        public static async Task MuteAsync(int pid)
        {
            await CallAsync($"{pid} --mute");
        }

        public static async Task UnmuteAsync(int pid)
        {
            await CallAsync($"{pid} --unmute");
        }

        private static async Task CallAsync(string Arguments)
        {
            Process? p = Process.Start(new ProcessStartInfo("vccli.exe")
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                Arguments = Arguments
            });
            await Task.Run(() => p?.WaitForExit());
        }
    }
}
