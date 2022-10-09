using System;
using System.Management;
using System.Runtime.InteropServices;

namespace GenshinWoodmen.Core
{
    internal static class NativeMethods
    {
        public const int WM_SYSCOMMAND = 0x0112;
        public const int WM_HOTKEY = 0x0312;

        public const int SC_RESTORE = 0xF120;

        public const int GWL_EXSTYLE = -20;
        public const int WS_EX_TOOLWINDOW = 0x00000080;

        public const uint ES_AWAYMODE_REQUIRED = 0x00000040;
        public const uint ES_CONTINUOUS = 0x80000000;
        public const uint ES_DISPLAY_REQUIRED = 0x00000002;
        public const uint ES_SYSTEM_REQUIRED = 0x00000001;

        public const int SE_PRIVILEGE_ENABLED = 0x00000002;
        public const int TOKEN_QUERY = 0x00000008;
        public const int TOKEN_ADJUST_PRIVILEGES = 0x00000020;
        public const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";
        public const int EWX_LOGOFF = 0x00000000;
        public const int EWX_SHUTDOWN = 0x00000001;
        public const int EWX_REBOOT = 0x00000002;
        public const int EWX_FORCE = 0x00000004;
        public const int EWX_POWEROFF = 0x00000008;
        public const int EWX_FORCEIFHUNG = 0x00000010;

        public const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        public const int WM_APPCOMMAND = 0x319;

        public const int MONITOR_DEFAULTTONULL = 0;
        public const int MONITOR_DEFAULTTOPRIMARY = 1;
        public const int MONITOR_DEFAULTTONEAREST = 2;

        public static uint SND_ASYNC = 0x0001;
        public static uint SND_FILENAME = 0x00020000;

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool BringWindowToTop(IntPtr hWnd);

        [DllImport("user32.dll")]
        public extern static bool SetCursorPos(int x, int y);

        [DllImport("User32.dll")]
        public static extern bool GetCursorPos(ref POINT lpPoint);

        [DllImport("user32.dll")]
        public static extern int GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [DllImport("kernel32.dll")]
        public static extern int SetThreadExecutionState(uint esFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern short GetAsyncKeyState(ushort virtualKeyCode);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern short GetKeyState(ushort virtualKeyCode);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint SendInput(uint numberOfInputs, INPUT[] inputs, int sizeOfInputStructure);

        [DllImport("user32.dll")]
        public static extern IntPtr GetMessageExtraInfo();

        [DllImport("kernel32.dll", ExactSpelling = true)]
        public static extern IntPtr GetCurrentProcess();

        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool OpenProcessToken(IntPtr h, int acc, ref IntPtr phtok);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LookupPrivilegeValue(string host, string name, ref long pluid);

        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool AdjustTokenPrivileges(IntPtr htok, bool disall, ref TokPriv1Luid newst, int len, IntPtr prev, IntPtr relen);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool ExitWindowsEx(int flg, int rea);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("gdi32.dll")]
        public static extern bool GetDeviceGammaRamp(IntPtr hDC, ref RAMP lpRamp);

        [DllImport("gdi32.dll")]
        public static extern bool SetDeviceGammaRamp(IntPtr hDC, ref RAMP lpRamp);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("dxva2.dll")]
        public static extern bool GetNumberOfPhysicalMonitorsFromHMONITOR(IntPtr hMonitor, ref uint pdwNumberOfPhysicalMonitors);
 
        [DllImport("dxva2.dll")]
        public static extern bool SetMonitorBrightness(IntPtr hMonitor, short brightness);
 
        [DllImport("dxva2.dll")]
        public static extern bool GetMonitorBrightness(IntPtr hMonitor, ref short pdwMinimumBrightness, ref short pdwCurrentBrightness, ref short pdwMaximumBrightness);

        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromPoint(POINT pt, uint dwFlags);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("winmm.dll", EntryPoint = "mciSendString")]
        public static extern uint MciSendString(string lpstrCommand, string lpstrReturnString, uint uReturnLength, uint hWndCallback);

        public static void Focus(IntPtr hwnd)
        {
            _ = SendMessage(hwnd, WM_SYSCOMMAND, SC_RESTORE, 0);
            _ = SetForegroundWindow(hwnd);
            while (IsIconic(hwnd))
            {
                continue;
            }
            _ = BringWindowToTop(hwnd);
        }

        public static void CursorCenterPos(IntPtr hwnd, int offsetX = 0, int offsetY = 0)
        {
            RECT rect = GetWindowRECT(hwnd);
            _ = SetCursorPos(rect.Left + (int)((rect.Right - rect.Left) / 2d) + offsetX, rect.Top + (int)((rect.Bottom - rect.Top) / 2d) + offsetY);
        }

        public static RECT GetWindowRECT(IntPtr hwnd)
        {
            RECT rect = new();
            _ = GetWindowRect(hwnd, ref rect);

            return rect;
        }

        public static void DoExitWin(int flg)
        {
            IntPtr hproc = GetCurrentProcess();
            IntPtr htok = IntPtr.Zero;
            TokPriv1Luid tpl = new()
            {
                Count = 1,
                Luid = 0,
                Attr = SE_PRIVILEGE_ENABLED,
            };
            OpenProcessToken(hproc, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, ref htok);
            LookupPrivilegeValue(null!, SE_SHUTDOWN_NAME, ref tpl.Luid);
            AdjustTokenPrivileges(htok, false, ref tpl, 0, IntPtr.Zero, IntPtr.Zero);
            ExitWindowsEx(flg, 0);
        }

        public static void ShutdownPowerOff()
        {
            DoExitWin(EWX_FORCE | EWX_POWEROFF);
        }

        public static void ShutdownReboot()
        {
            DoExitWin(EWX_FORCE | EWX_REBOOT);
        }

        public static void ShutdownLogoff()
        {
            DoExitWin(EWX_FORCE | EWX_LOGOFF);
        }

        public static void SetDeviceGamma(int gamma)
        {
            if (gamma < 3) gamma = 3;
            if (gamma > 44) gamma = 44;
            RAMP ramp = new()
            {
                Red = new ushort[256],
                Green = new ushort[256],
                Blue = new ushort[256],
            };
            for (int i = 1; i < 256; i++)
            {
                ramp.Red[i] = ramp.Green[i] = ramp.Blue[i] = (ushort)Math.Min(ushort.MaxValue, Math.Max(ushort.MinValue, Math.Pow((i + 1d) / 256d, gamma * 0.1d) * ushort.MaxValue + 0.5d));
            }
            SetDeviceGammaRamp(GetDC(IntPtr.Zero), ref ramp);
        }

        public static int GetDeviceGamma()
        {
            RAMP ramp = new()
            {
                Red = new ushort[256],
                Green = new ushort[256],
                Blue = new ushort[256],
            };
            _ = GetDeviceGammaRamp(GetDC(IntPtr.Zero), ref ramp);
            return default;
        }

        public static IntPtr GetCurrentMonitor()
        {
            POINT p = new(0, 0);
            _ = GetCursorPos(ref p);
            return MonitorFromPoint(p, MONITOR_DEFAULTTOPRIMARY);
        }

        [Obsolete]
        public static void SetMonitorBrightness(short brightness)
        {
            IntPtr hMonitor = GetCurrentMonitor();
            _ = SetMonitorBrightness(hMonitor, brightness);
        }

        [Obsolete]
        public static short GetMonitorBrightness()
        {
            IntPtr hMonitor = GetCurrentMonitor();
            short pdwMinimumBrightness = default;
            short pdwCurrentBrightness = default;
            short pdwMaximumBrightness = default;
            _ = GetMonitorBrightness(hMonitor, ref pdwMinimumBrightness, ref pdwCurrentBrightness, ref pdwMaximumBrightness);
            return pdwCurrentBrightness;
        }

        [Obsolete]
        public static short GetMonitorMinimumBrightness()
        {
            IntPtr hMonitor = GetCurrentMonitor();
            short pdwMinimumBrightness = default;
            short pdwCurrentBrightness = default;
            short pdwMaximumBrightness = default;
            _ = GetMonitorBrightness(hMonitor, ref pdwMinimumBrightness, ref pdwCurrentBrightness, ref pdwMaximumBrightness);
            return pdwMinimumBrightness;
        }

        [Obsolete]
        public static short GetMonitorMaximumBrightness()
        {
            IntPtr hMonitor = GetCurrentMonitor();
            short pdwMinimumBrightness = default;
            short pdwCurrentBrightness = default;
            short pdwMaximumBrightness = default;
            _ = GetMonitorBrightness(hMonitor, ref pdwMinimumBrightness, ref pdwCurrentBrightness, ref pdwMaximumBrightness);
            return pdwMaximumBrightness;
        }

        public static byte GetBrightness()
        {
            ManagementScope scope = new(@"root\WMI");
            ObjectQuery query = new("SELECT * FROM WmiMonitorBrightness");
            using ManagementObjectSearcher searcher = new(scope, query);
            using ManagementObjectCollection objectCollection = searcher.Get();

            foreach (ManagementObject mObj in objectCollection)
            {
                return byte.Parse(mObj["CurrentBrightness"].ToString()!);
            }
            return 0x00;
        }

        public static void SetBrightness(byte targetBrightness)
        {
            ManagementScope scope = new(@"root\WMI");
            SelectQuery query = new("WmiMonitorBrightnessMethods");
            using ManagementObjectSearcher searcher = new(scope, query);
            using ManagementObjectCollection objectCollection = searcher.Get();

            foreach (ManagementObject mObj in objectCollection)
            {
                mObj.InvokeMethod("WmiSetBrightness", new object[] { uint.MaxValue, targetBrightness });
                break;
            }
        }

        public static void SetToolWindow(IntPtr hwnd)
        {
            int style = GetWindowLong(hwnd, GWL_EXSTYLE);

            style |= WS_EX_TOOLWINDOW;
            SetWindowLong(hwnd, GWL_EXSTYLE, style);
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    internal struct HARDWAREINPUT
    {
        public uint Msg;
        public ushort ParamL;
        public ushort ParamH;
    }

    internal struct INPUT
    {
        public uint Type;
        public MOUSEKEYBDHARDWAREINPUT Data;
    }

    internal enum InputType : uint
    {
        Mouse,
        Keyboard,
        Hardware
    }

    internal struct KEYBDINPUT
    {
        public ushort KeyCode;
        public ushort Scan;
        public uint Flags;
        public uint Time;
        public IntPtr ExtraInfo;
    }

    [Flags]
    internal enum KeyboardFlag : uint
    {
        ExtendedKey = 1U,
        KeyUp = 2U,
        Unicode = 4U,
        ScanCode = 8U
    }

    [Flags]
    internal enum MouseFlag : uint
    {
        Move = 1U,
        LeftDown = 2U,
        LeftUp = 4U,
        RightDown = 8U,
        RightUp = 16U,
        MiddleDown = 32U,
        MiddleUp = 64U,
        XDown = 128U,
        XUp = 256U,
        VerticalWheel = 2048U,
        HorizontalWheel = 4096U,
        VirtualDesk = 16384U,
        Absolute = 32768U
    }

    internal struct MOUSEINPUT
    {
        public int X;
        public int Y;
        public uint MouseData;
        public uint Flags;
        public uint Time;
        public IntPtr ExtraInfo;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct MOUSEKEYBDHARDWAREINPUT
    {
        [FieldOffset(0)]
        public MOUSEINPUT Mouse;
        [FieldOffset(0)]
        public KEYBDINPUT Keyboard;
        [FieldOffset(0)]
        public HARDWAREINPUT Hardware;
    }

    internal enum XButton : uint
    {
        XButton1 = 1U,
        XButton2
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TokPriv1Luid
    {
        public int Count;
        public long Luid;
        public int Attr;
    }

    public enum VirtualKeyCode
    {
        LBUTTON = 1,
        RBUTTON,
        CANCEL,
        MBUTTON,
        XBUTTON1,
        XBUTTON2,
        BACK = 8,
        TAB,
        CLEAR = 12,
        RETURN,
        SHIFT = 16,
        CONTROL,
        MENU,
        PAUSE,
        CAPITAL,
        KANA,
        HANGEUL = 21,
        HANGUL = 21,
        JUNJA = 23,
        FINAL,
        HANJA,
        KANJI = 25,
        ESCAPE = 27,
        CONVERT,
        NONCONVERT,
        ACCEPT,
        MODECHANGE,
        SPACE,
        PRIOR,
        NEXT,
        END,
        HOME,
        LEFT,
        UP,
        RIGHT,
        DOWN,
        SELECT,
        PRINT,
        EXECUTE,
        SNAPSHOT,
        INSERT,
        DELETE,
        HELP,
        VK_0,
        VK_1,
        VK_2,
        VK_3,
        VK_4,
        VK_5,
        VK_6,
        VK_7,
        VK_8,
        VK_9,
        VK_A = 65,
        VK_B,
        VK_C,
        VK_D,
        VK_E,
        VK_F,
        VK_G,
        VK_H,
        VK_I,
        VK_J,
        VK_K,
        VK_L,
        VK_M,
        VK_N,
        VK_O,
        VK_P,
        VK_Q,
        VK_R,
        VK_S,
        VK_T,
        VK_U,
        VK_V,
        VK_W,
        VK_X,
        VK_Y,
        VK_Z,
        LWIN,
        RWIN,
        APPS,
        SLEEP = 95,
        NUMPAD0,
        NUMPAD1,
        NUMPAD2,
        NUMPAD3,
        NUMPAD4,
        NUMPAD5,
        NUMPAD6,
        NUMPAD7,
        NUMPAD8,
        NUMPAD9,
        MULTIPLY,
        ADD,
        SEPARATOR,
        SUBTRACT,
        DECIMAL,
        DIVIDE,
        F1,
        F2,
        F3,
        F4,
        F5,
        F6,
        F7,
        F8,
        F9,
        F10,
        F11,
        F12,
        F13,
        F14,
        F15,
        F16,
        F17,
        F18,
        F19,
        F20,
        F21,
        F22,
        F23,
        F24,
        NUMLOCK = 144,
        SCROLL,
        LSHIFT = 160,
        RSHIFT,
        LCONTROL,
        RCONTROL,
        LMENU,
        RMENU,
        BROWSER_BACK,
        BROWSER_FORWARD,
        BROWSER_REFRESH,
        BROWSER_STOP,
        BROWSER_SEARCH,
        BROWSER_FAVORITES,
        BROWSER_HOME,
        VOLUME_MUTE,
        VOLUME_DOWN,
        VOLUME_UP,
        MEDIA_NEXT_TRACK,
        MEDIA_PREV_TRACK,
        MEDIA_STOP,
        MEDIA_PLAY_PAUSE,
        LAUNCH_MAIL,
        LAUNCH_MEDIA_SELECT,
        LAUNCH_APP1,
        LAUNCH_APP2,
        OEM_1 = 186,
        OEM_PLUS,
        OEM_COMMA,
        OEM_MINUS,
        OEM_PERIOD,
        OEM_2,
        OEM_3,
        OEM_4 = 219,
        OEM_5,
        OEM_6,
        OEM_7,
        OEM_8,
        OEM_102 = 226,
        PROCESSKEY = 229,
        PACKET = 231,
        ATTN = 246,
        CRSEL,
        EXSEL,
        EREOF,
        PLAY,
        ZOOM,
        NONAME,
        PA1,
        OEM_CLEAR
    }

    [Flags]
    public enum ModifierKeys : uint
    {
        Alt = 1,
        Control = 2,
        Shift = 4,
        Win = 8
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct RAMP
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public ushort[] Red;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public ushort[] Green;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public ushort[] Blue;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int x;
        public int y;
        public POINT(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
