using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Utilities.Mouse_and_Keyboard;

namespace Ayazis.KeyHooks;

public interface IMouseAndKeysCapture
{
    event EventHandler MouseButtonClicked;

    void HookIntoMouseAndKeyBoard();
}

public class MouseAndKeysCapture : IMouseAndKeysCapture
{
    private LowLevelKeyboardProc _keyboardProc;
    private LowLevelMouseProc _mouseProc;
    private IntPtr _keyboardHookID = IntPtr.Zero;
    private IntPtr _mouseHookId = IntPtr.Zero;
    private IKeyInputHandler _keyInputHandler;

    public event EventHandler MouseButtonClicked;
    public MouseAndKeysCapture(IKeyInputHandler keyInputHandler)
    {
        _keyboardProc = KeyBoardHookCallback;
        _mouseProc = MouseHookCallBack;
        _keyInputHandler = keyInputHandler;
    }

    public void HookIntoMouseAndKeyBoard()
    {
        using (Process curProcess = Process.GetCurrentProcess())
        using (ProcessModule curModule = curProcess.MainModule)
        {
            _keyboardHookID = SetWindowsHookEx(WH_KEYBOARD_LL, _keyboardProc, GetModuleHandle(curModule.ModuleName), 0);
            _mouseHookId = SetWindowsHookEx(WH_MOUSE_LL, _mouseProc, GetModuleHandle(curModule.ModuleName), 0);
        }
    }

    private IntPtr MouseHookCallBack(int nCode, IntPtr wParam, IntPtr lParam)
    {
        // If nCode is less than 0, the hook procedure must pass the message to the CallNextHookEx function.
        if (nCode < 0)
            return CallNextHookEx(_mouseHookId, nCode, wParam, lParam);

        MouseMessages input = (MouseMessages)wParam;


        switch (input)
        {
            case MouseMessages.WM_LBUTTONDOWN:
            case MouseMessages.WM_RBUTTONDOWN:
                // Check if the specified key is pressed
                bool keyCombinationHit = KeyPressed(input == MouseMessages.WM_LBUTTONDOWN ? Keys.LButton : Keys.RButton);
                // If a key combination is hit, prevent other software or Windows from receiving the key input
                if (keyCombinationHit)
                    return IntPtr.Zero;
                else
                    MouseButtonClicked?.Invoke(this, null); // Raise the MouseButtonClicked event, we can check if the UI is active, and if not, hide it.
                break;

            case MouseMessages.WM_LBUTTONUP:
            case MouseMessages.WM_RBUTTONUP:
                // Mark the specified key as released
                KeyReleased(input == MouseMessages.WM_LBUTTONUP ? Keys.LButton : Keys.RButton);
                break;
        }
        // Pass the message to the next hook in the chain
        return CallNextHookEx(_mouseHookId, nCode, wParam, lParam);
    }
    private IntPtr KeyBoardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        // If nCode is less than 0, the hook procedure must pass the message to the CallNextHookEx function.
        if (nCode < 0)
            return CallNextHookEx(_keyboardHookID, nCode, wParam, lParam);


        // Read the virtual key code from the lParam
        int vkCode = Marshal.ReadInt32(lParam);
        Keys key = (Keys)vkCode;

        if (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN)
        {
            // Check if the specified key is pressed
            bool keyCombinationHit = KeyPressed(key);
            // If a key combination is hit, prevent other software or Windows from receiving the key input
            if (keyCombinationHit)
            {
                return IntPtr.Zero;
            }
        }
        else if (wParam == (IntPtr)WM_KEYUP)
        {
            // Mark the specified key as released
            KeyReleased(key);
        }

        // Pass the message to the next hook in the chain
        return CallNextHookEx(_keyboardHookID, nCode, wParam, lParam);
    }
    bool KeyPressed(Keys key)
    {
        return _keyInputHandler.IsPresetKeyCombinationHit(key);
    }

    void KeyReleased(Keys key)
    {
        _keyInputHandler.KeyReleased(key);
    }

    private enum MouseMessages
    {
        WM_LBUTTONDOWN = 0x0201,
        WM_LBUTTONUP = 0x0202,
        WM_MOUSEMOVE = 0x0200,
        WM_MOUSEWHEEL = 0x020A,
        WM_RBUTTONDOWN = 0x0204,
        WM_RBUTTONUP = 0x0205
    }
    private const int WH_MOUSE_LL = 14;
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_KEYUP = 0x101;
    private const int WM_SYSKEYDOWN = 0x0104;

    #region dllImports
    internal delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
    internal delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);


    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool UnhookWindowsHookEx(IntPtr hhk);


    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);


    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);
    #endregion
}