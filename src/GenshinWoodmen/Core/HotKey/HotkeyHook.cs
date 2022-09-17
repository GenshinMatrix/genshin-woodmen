using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GenshinWoodmen.Core
{
    /// <summary>
    /// https://stackoverflow.com/questions/2450373/set-global-hotkeys-using-c-sharp
    /// </summary>
    public sealed class HotkeyHook : IDisposable
    {
        public event EventHandler<KeyPressedEventArgs>? KeyPressed;
        private readonly Window? window = new();
        private int currentId;

        private class Window : NativeWindow, IDisposable
        {
            public event EventHandler<KeyPressedEventArgs>? KeyPressed;

            public Window()
            {
                CreateHandle(new CreateParams());
            }

            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);

                if (m.Msg == NativeMethods.WM_HOTKEY)
                {
                    Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                    ModifierKeys modifier = (ModifierKeys)((int)m.LParam & 0xFFFF);

                    KeyPressed?.Invoke(this, new KeyPressedEventArgs(modifier, key));
                }
            }

            public void Dispose()
            {
                DestroyHandle();
            }
        }

        public HotkeyHook()
        {
            window.KeyPressed += (sender, args) =>
            {
                KeyPressed?.Invoke(this, args);
            };
        }

        public void RegisterHotKey(ModifierKeys modifier, Keys key)
        {
            currentId += 1;
            if (!NativeMethods.RegisterHotKey(window!.Handle, currentId, (uint)modifier, (uint)key))
            {
                if (Marshal.GetLastWin32Error() == 1409)
                    throw new InvalidOperationException("Hotkey already occupied");
                else
                    throw new InvalidOperationException("Hotkey registration failed");
            }
        }

        public void UnregisterHotKey()
        {
            for (int i = currentId; i > 0; i--)
            {
                NativeMethods.UnregisterHotKey(window!.Handle, i);
            }
        }

        public void Dispose()
        {
            UnregisterHotKey();
            window?.Dispose();
        }
    }

    public class KeyPressedEventArgs : EventArgs
    {
        public ModifierKeys Modifier { get; }
        public Keys Key { get; }

        internal KeyPressedEventArgs(ModifierKeys modifier, Keys key)
        {
            Modifier = modifier;
            Key = key;
        }
    }
}
