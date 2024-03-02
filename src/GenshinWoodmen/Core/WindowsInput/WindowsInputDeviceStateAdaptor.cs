namespace GenshinWoodmen.Core;

public class WindowsInputDeviceStateAdaptor : IInputDeviceStateAdaptor
{
    public bool IsKeyDown(VirtualKeyCode keyCode)
    {
        short keyState = NativeMethods.GetKeyState((ushort)keyCode);
        return keyState < 0;
    }

    public bool IsKeyUp(VirtualKeyCode keyCode)
    {
        return !IsKeyDown(keyCode);
    }

    public bool IsHardwareKeyDown(VirtualKeyCode keyCode)
    {
        short asyncKeyState = NativeMethods.GetAsyncKeyState((ushort)keyCode);
        return asyncKeyState < 0;
    }

    public bool IsHardwareKeyUp(VirtualKeyCode keyCode)
    {
        return !IsHardwareKeyDown(keyCode);
    }

    public bool IsTogglingKeyInEffect(VirtualKeyCode keyCode)
    {
        short keyState = NativeMethods.GetKeyState((ushort)keyCode);
        return (keyState & 1) == 1;
    }
}
