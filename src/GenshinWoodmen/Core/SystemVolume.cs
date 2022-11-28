using System;
using System.Runtime.InteropServices;

/// <summary>
/// https://stackoverflow.com/questions/14306048/controlling-volume-mixer
/// </summary>
namespace GenshinWoodmen.Core;

public static class SystemVolume
{
    public static float GetMasterVolume()
    {
        IAudioEndpointVolume masterVol = null!;

        try
        {
            masterVol = GetMasterVolumeObject();
            if (masterVol == null)
                return -1f;

            masterVol.GetMasterVolumeLevelScalar(out var volumeLevel);
            return volumeLevel * 100f;
        }
        finally
        {
            if (masterVol != null)
                Marshal.ReleaseComObject(masterVol);
        }
    }

    public static void SetMasterVolume(float newLevel)
    {
        IAudioEndpointVolume masterVol = null!;
        try
        {
            masterVol = GetMasterVolumeObject();
            if (masterVol == null)
                return;

            masterVol.SetMasterVolumeLevelScalar(newLevel / 100f, Guid.Empty);
        }
        finally
        {
            if (masterVol != null)
                Marshal.ReleaseComObject(masterVol);
        }
    }

    public static void SetMasterVolumeMute(bool isMuted)
    {
        IAudioEndpointVolume masterVol = null!;

        try
        {
            masterVol = GetMasterVolumeObject();
            if (masterVol == null)
                return;

            masterVol.SetMute(isMuted, Guid.Empty);
        }
        finally
        {
            if (masterVol != null)
                Marshal.ReleaseComObject(masterVol);
        }
    }

    private static IAudioEndpointVolume GetMasterVolumeObject()
    {
        IMMDeviceEnumerator deviceEnumerator = null!;
        IMMDevice speakers = null!;

        try
        {
            deviceEnumerator = (IMMDeviceEnumerator)new MMDeviceEnumerator();
            deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia, out speakers);

            Guid iidIAudioEndpointVolume = typeof(IAudioEndpointVolume).GUID;
            speakers.Activate(ref iidIAudioEndpointVolume, 0, IntPtr.Zero, out object o);
            IAudioEndpointVolume masterVol = (IAudioEndpointVolume)o;

            return masterVol;
        }
        finally
        {
            if (speakers != null) Marshal.ReleaseComObject(speakers);
            if (deviceEnumerator != null) Marshal.ReleaseComObject(deviceEnumerator);
        }
    }
}

[ComImport]
[Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
internal class MMDeviceEnumerator
{
}

internal enum EDataFlow
{
    eRender,
    eCapture,
    eAll,
    EDataFlow_enum_count
}

internal enum ERole
{
    eConsole,
    eMultimedia,
    eCommunications,
    ERole_enum_count
}

[Guid("A95664D2-9614-4F35-A746-DE8DB63617E6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IMMDeviceEnumerator
{
    int NotImpl1();

    [PreserveSig]
    int GetDefaultAudioEndpoint(EDataFlow dataFlow, ERole role, out IMMDevice ppDevice);
}

[Guid("D666063F-1587-4E43-81F1-B948E807363F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IMMDevice
{
    [PreserveSig]
    int Activate(ref Guid iid, int dwClsCtx, IntPtr pActivationParams, [MarshalAs(UnmanagedType.IUnknown)] out object ppInterface);
}

[Guid("E2F5BB11-0570-40CA-ACDD-3AA01277DEE8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IAudioSessionEnumerator
{
    [PreserveSig]
    int GetCount(out int SessionCount);

    [PreserveSig]
    int GetSession(int SessionCount, out IAudioSessionControl2 Session);
}

[Guid("bfb7ff88-7239-4fc9-8fa2-07c950be9c6d"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IAudioSessionControl2
{
    [PreserveSig]
    int NotImpl0();

    [PreserveSig]
    int GetDisplayName([MarshalAs(UnmanagedType.LPWStr)] out string pRetVal);

    [PreserveSig]
    int SetDisplayName([MarshalAs(UnmanagedType.LPWStr)] string Value, [MarshalAs(UnmanagedType.LPStruct)] Guid EventContext);

    [PreserveSig]
    int GetIconPath([MarshalAs(UnmanagedType.LPWStr)] out string pRetVal);

    [PreserveSig]
    int SetIconPath([MarshalAs(UnmanagedType.LPWStr)] string Value, [MarshalAs(UnmanagedType.LPStruct)] Guid EventContext);

    [PreserveSig]
    int GetGroupingParam(out Guid pRetVal);

    [PreserveSig]
    int SetGroupingParam([MarshalAs(UnmanagedType.LPStruct)] Guid Override, [MarshalAs(UnmanagedType.LPStruct)] Guid EventContext);

    [PreserveSig]
    int NotImpl1();

    [PreserveSig]
    int NotImpl2();

    [PreserveSig]
    int GetSessionIdentifier([MarshalAs(UnmanagedType.LPWStr)] out string pRetVal);

    [PreserveSig]
    int GetSessionInstanceIdentifier([MarshalAs(UnmanagedType.LPWStr)] out string pRetVal);

    [PreserveSig]
    int GetProcessId(out int pRetVal);

    [PreserveSig]
    int IsSystemSoundsSession();

    [PreserveSig]
    int SetDuckingPreference(bool optOut);
}

/// <summary>
/// http://netcoreaudio.codeplex.com/SourceControl/latest#trunk/Code/CoreAudio/Interfaces/IAudioEndpointVolume.cs
/// </summary>
[Guid("5CDF2C82-841E-4546-9722-0CF74078229A"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IAudioEndpointVolume
{
    [PreserveSig]
    int NotImpl1();

    [PreserveSig]
    int NotImpl2();

    [PreserveSig]
    int GetChannelCount([Out][MarshalAs(UnmanagedType.U4)] out uint channelCount);

    [PreserveSig]
    int SetMasterVolumeLevel([In][MarshalAs(UnmanagedType.R4)] float level, [In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

    [PreserveSig]
    int SetMasterVolumeLevelScalar([In][MarshalAs(UnmanagedType.R4)] float level, [In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

    [PreserveSig]
    int GetMasterVolumeLevel([Out][MarshalAs(UnmanagedType.R4)] out float level);

    [PreserveSig]
    int GetMasterVolumeLevelScalar([Out][MarshalAs(UnmanagedType.R4)] out float level);

    [PreserveSig]
    int SetChannelVolumeLevel([In][MarshalAs(UnmanagedType.U4)] uint channelNumber, [In][MarshalAs(UnmanagedType.R4)] float level, [In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

    [PreserveSig]
    int SetChannelVolumeLevelScalar([In][MarshalAs(UnmanagedType.U4)] uint channelNumber, [In][MarshalAs(UnmanagedType.R4)] float level, [In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

    [PreserveSig]
    int GetChannelVolumeLevel([In][MarshalAs(UnmanagedType.U4)] uint channelNumber, [Out][MarshalAs(UnmanagedType.R4)] out float level);

    [PreserveSig]
    int GetChannelVolumeLevelScalar([In][MarshalAs(UnmanagedType.U4)] uint channelNumber, [Out][MarshalAs(UnmanagedType.R4)] out float level);

    [PreserveSig]
    int SetMute([In][MarshalAs(UnmanagedType.Bool)] bool isMuted, [In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

    [PreserveSig]
    int GetMute([Out][MarshalAs(UnmanagedType.Bool)] out bool isMuted);

    [PreserveSig]
    int GetVolumeStepInfo([Out][MarshalAs(UnmanagedType.U4)] out uint step, [Out][MarshalAs(UnmanagedType.U4)] out uint stepCount);

    [PreserveSig]
    int VolumeStepUp([In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

    [PreserveSig]
    int VolumeStepDown([In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

    [PreserveSig]
    int QueryHardwareSupport([Out][MarshalAs(UnmanagedType.U4)] out uint hardwareSupportMask);

    [PreserveSig]
    int GetVolumeRange([Out][MarshalAs(UnmanagedType.R4)] out float volumeMin, [Out][MarshalAs(UnmanagedType.R4)] out float volumeMax, [Out][MarshalAs(UnmanagedType.R4)] out float volumeStep);
}
