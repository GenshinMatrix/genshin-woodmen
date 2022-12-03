using CommunityToolkit.Mvvm.Messaging;
using GenshinWoodmen.Models;
using GenshinWoodmen.Views;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GenshinWoodmen.Core;

internal static class JiggingProcessor
{
    public static bool IsCanceled { get; private set; } = false;
    public static Thread ProcessingThread { get; private set; } = null!;

    public static void Start()
    {
        IsCanceled = false;
        ProcessingThread = new(Processing)
        {
            IsBackground = true,
        };
        ProcessingThread.Start();
        NativeMethods.SetThreadExecutionState(NativeMethods.ES_CONTINUOUS | NativeMethods.ES_SYSTEM_REQUIRED | NativeMethods.ES_AWAYMODE_REQUIRED);
    }

    public static void Stop()
    {
        IsCanceled = true;
        ProcessingThread = null!;
        NativeMethods.SetThreadExecutionState(NativeMethods.ES_CONTINUOUS);
    }

    public static async Task Delay(int millisecondsDelay)
    {
        _ = await Task.Run(() => SpinWait.SpinUntil(() => IsCanceled, millisecondsDelay));
        if (IsCanceled) throw new TimeoutException("User Aborted");
    }

    public static void Count()
    {
        WeakReferenceMessenger.Default.Send(new CountMessage());
    }

    public static void TraceStatus(string message)
    {
        Logger.Info(message);
        WeakReferenceMessenger.Default.Send(new StatusMessage(message));
    }

    public static async void Processing()
    {
        bool lastAutoLogout = false;

        CompatibleEntry.Ensure();
        await SetMute(true);
        while (!IsCanceled)
        {
            try
            {
                IntPtr hwnd = IntPtr.Zero;

                if (!await LaunchCtrl.IsRunning(async p => hwnd = p?.MainWindowHandle ?? IntPtr.Zero))
                {
                    TraceStatus("Launching");
                    _ = await LaunchCtrl.Launch(relaunchMethod: RelaunchMethod.Logout);
                    TraceStatus("Launch(1st) Chattering");
                    await Delay(Settings.DelayLaunchFirst.Get());
                    TraceStatus("Launch(1st) Chattered");
                    await LaunchCtrl.IsRunning(async p => hwnd = p?.MainWindowHandle ?? IntPtr.Zero);
                    await Delay(1000);
                    TraceStatus("FocusWindow");
                    NativeMethods.Focus(hwnd);
                    NativeMethods.CursorCenterPos(hwnd);
                    UserSimulator.Input.Mouse.LeftButtonClick();
                    TraceStatus("Click LeftMouse");
                    await Delay(100);
                    TraceStatus("Login(1st) Chattering");
                    if ((AutoMuteSelection)Settings.AutoMute.Get() == AutoMuteSelection.AutoMuteGame) await MuteManager.MuteGameAsync(true);
                    await Delay(Settings.DelayLoginFirst.Get());
                    TraceStatus("Login(1st) Chattered");
                    TraceStatus("Focus Window");
                    NativeMethods.Focus(hwnd);
                    NativeMethods.CursorCenterPos(hwnd);
                    UserSimulator.Input.Keyboard.KeyPress(VirtualKeyCode.VK_Z);
                    TraceStatus("Pressed KeyZ");
                    Count();
                    await Delay(200);
                    TraceStatus("Got Wood");
                }
                else
                {
                    if (lastAutoLogout)
                    {
                        if (CompatibleBilibili.IsAvailabled)
                        {
                            TraceStatus("Login Compatible for bilibili");
                            await Delay(Settings.DelayLaunch.Get() / 2);
                            await CompatibleBilibili.Login();
                            TraceStatus("Login Logined for bilibili");
                        }
                        TraceStatus("Login Chattering");
                        await Delay(Settings.DelayLaunch.Get());
                        TraceStatus("Login Chattered");
                        await Delay(1000);
                        TraceStatus("FocusWindow");
                        NativeMethods.Focus(hwnd);
                        NativeMethods.CursorCenterPos(hwnd);
                        UserSimulator.Input.Mouse.LeftButtonClick();
                        TraceStatus("Click LeftMouse");
                        TraceStatus("Launch Chattering");
                        await Delay(Settings.DelayLogin.Get());
                        TraceStatus("Launch Chattered");
                    }
                    TraceStatus("Launched");
                    await Delay(1000);
                    TraceStatus("Focus Window");
                    NativeMethods.Focus(hwnd);
                    NativeMethods.CursorCenterPos(hwnd);
                    UserSimulator.Input.Keyboard.KeyPress(VirtualKeyCode.VK_Z);
                    TraceStatus("Pressed KeyZ");
                    Count();
                    await Delay(200);
                    TraceStatus("Got Wood");
                    await Delay(800);
                    TraceStatus("Logout");
                    await LaunchCtrl.Logout();
                    lastAutoLogout = true;
                    TraceStatus("Logouted");
                    await Delay(1000);
                }
            }
            catch (TimeoutException e)
            {
                TraceStatus(e.Message);
            }
            catch (Exception e)
            {
                Logger.Exception(e);
                NoticeService.AddNotice(Mui("Tips"), "Failed", e.Message);
            }
        }
        await SetMute(false);
    }

    public static async Task SetMute(bool isMuted)
    {
        switch ((AutoMuteSelection)Settings.AutoMute.Get())
        {
            case AutoMuteSelection.AutoMuteSystem:
                MuteManager.MuteSystem(isMuted);
                break;
            case AutoMuteSelection.AutoMuteGame:
                await MuteManager.MuteGameAsync(isMuted);
                break;
        }
    }
}
