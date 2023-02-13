using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using QuickPick.Logic;
using QuickPick;
using System.Windows;
using Gma.System.MouseKeyHook;

internal static class KeyHook
{
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_KEYUP = 0x101;
    private const int WM_SYSKEYDOWN = 0x0104;

    internal static LowLevelKeyboardProc _proc = HookCallback;
    internal static IntPtr _hookID = IntPtr.Zero;


    internal static void SetKeyboardHook()
    {

        using (Process curProcess = Process.GetCurrentProcess())

        using (ProcessModule curModule = curProcess.MainModule)

        {

			_hookID = SetWindowsHookEx(WH_KEYBOARD_LL, _proc,

                GetModuleHandle(curModule.ModuleName), 0);

        }
    }

    internal static void SetMouseHooks()
    {
        Hook.GlobalEvents().MouseDown += MouseDown;
        Hook.GlobalEvents().MouseUp += MouseUp;
    }

	private static void MouseDown(object sender, MouseEventArgs e)
	{
		Keys key = e.Button == MouseButtons.Left ? Keys.LButton : Keys.RButton;
		HotKeys.KeyDowned(key);
	}


	private static void MouseUp(object sender, MouseEventArgs e)
	{
		Keys key = e.Button == MouseButtons.Left ? Keys.LButton : Keys.RButton;
		HotKeys.KeyUpped(key);
	}



	internal delegate IntPtr LowLevelKeyboardProc(

        int nCode, IntPtr wParam, IntPtr lParam);


    private static IntPtr HookCallback(
        int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN)
        {
            int vkCode = Marshal.ReadInt32(lParam);

            //Debug.WriteLine("DOWN: "+(Keys)vkCode);            
            HotKeys.KeyDowned((Keys)vkCode);

        }
        else if (wParam ==(IntPtr)WM_KEYUP)
        {
            int vkCode = Marshal.ReadInt32(lParam);
			//Debug.WriteLine("Up: "+(Keys)vkCode);
			HotKeys.KeyUpped((Keys)vkCode);
        }

        return CallNextHookEx(_hookID, nCode, wParam, lParam);

    }


    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

    private static extern IntPtr SetWindowsHookEx(int idHook,

        LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);


    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

    [return: MarshalAs(UnmanagedType.Bool)]

    internal static extern bool UnhookWindowsHookEx(IntPtr hhk);


    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,

        IntPtr wParam, IntPtr lParam);


    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]

    private static extern IntPtr GetModuleHandle(string lpModuleName);

}