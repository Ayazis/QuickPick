using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Utilities.Mouse_and_Keyboard;

namespace Ayazis.KeyHooks;

public class MouseAndKeysCapture
{
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

	internal LowLevelKeyboardProc _keyboardProc;
	internal LowLevelMouseProc _mouseProc;
	internal IntPtr _keyboardHookID = IntPtr.Zero;
	internal IntPtr _mouseHookId = IntPtr.Zero;
	IKeyInputHandler _keyInputHandler;

	public MouseAndKeysCapture(IKeyInputHandler keyInputHandler)
	{
		_keyInputHandler = keyInputHandler;
		_keyboardProc = KeyBoardHookCallback;
		_mouseProc = MouseHookCallBack;
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
		bool keyCombinationHit = false;
		if (nCode < 0)
			return CallNextHookEx(_mouseHookId, nCode, wParam, lParam);

		MouseMessages input = (MouseMessages)wParam;

		if (input == MouseMessages.WM_LBUTTONDOWN)
			keyCombinationHit = KeyPressed(Keys.LButton);

		else if (input == MouseMessages.WM_RBUTTONDOWN)
			keyCombinationHit = KeyPressed(Keys.RButton);

		if (input == MouseMessages.WM_LBUTTONUP)
			KeyReleased(Keys.LButton);

		else if (input == MouseMessages.WM_RBUTTONUP)
			KeyReleased(Keys.RButton);

		if (keyCombinationHit)
		{			
			return IntPtr.Zero; // Do not forward event, this will prevent other software (or windows) to do something with the KeyInput.
		}

		return CallNextHookEx(_mouseHookId, nCode, wParam, lParam);
	}

	private IntPtr KeyBoardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
	{
		bool keyCombinationHit = false;

		if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN)
		{
			int vkCode = Marshal.ReadInt32(lParam);
			keyCombinationHit = KeyPressed((Keys)vkCode);
		}
		else if (wParam == (IntPtr)WM_KEYUP)
		{
			int vkCode = Marshal.ReadInt32(lParam);
			KeyReleased((Keys)vkCode);
		}
		if (keyCombinationHit)
		{
			return IntPtr.Zero; // Do not forward event, this will prevent other software (or windows) to do something with the KeyInput.
		}
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