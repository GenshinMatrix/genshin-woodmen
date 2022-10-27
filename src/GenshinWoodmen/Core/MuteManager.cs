using NAudio.CoreAudioApi;
using System;
using System.Threading.Tasks;
using MMDevices = NAudio.CoreAudioApi.MMDeviceEnumerator;

namespace GenshinWoodmen.Core
{
    internal class MuteManager
    {
        public static async Task MuteGameAsync(bool isMuted)
        {
            await LaunchCtrl.IsRunning(async p =>
            {
                await MuteProcessAsync(p!.Id, isMuted);
            });
        }

        private static async Task MuteProcessAsync(int pid, bool isMuted)
        {
            try
            {
                await Task.Run(() =>
                {
                    MuteProcess(pid, isMuted);
                });
            }
            catch (Exception e)
            {
                Logger.Exception(e);
            }
        }

        private static void MuteProcess(int pid, bool isMuted)
        {
            MMDevices audio = new();
            foreach (MMDevice device in audio.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active))
            {
                for (int i = default; i < device.AudioSessionManager.Sessions.Count; i++)
                {
                    AudioSessionControl session = device.AudioSessionManager.Sessions[i];

                    if (session.GetProcessID == pid)
                    {
                        session.SimpleAudioVolume.Mute = isMuted;
                        break;
                    }
                }
            }
        }

        public static void MuteSystem(bool isMuted)
        {
            try
            {
                SystemVolume.SetMasterVolumeMute(isMuted);
            }
            catch
            {
            }
        }
    }
}
