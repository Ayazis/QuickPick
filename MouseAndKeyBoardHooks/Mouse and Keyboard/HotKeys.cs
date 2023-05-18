
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Ayazis.KeyHooks;
public static class HotKeys
{
	static List<Keys> PressedKeys = new List<Keys>();
	static List<Keys> _presetKeyCombination = new List<Keys>();

	/// <summary>
	/// Hooks into the keyboard and the mouse events. 
	/// </summary>
	/// <param name="presetKeyCombination">The list of keys that need to be hit for the KeyCombinationHit to be raised.</param>
	public static void SubscribeToKeyEvents(IEnumerable<Keys> presetKeyCombination)
	{
		// Set predefined KeyCombination.
		_presetKeyCombination = presetKeyCombination.ToList();

		// Connect House and Keyboard input events to current class.
		CaptureKeyBoardAndMouse.HookIntoMouseAndKeyBoard();
		CaptureKeyBoardAndMouse.KeyDowned += CaptureKeyBoardAndMouse_KeyDowned;
		CaptureKeyBoardAndMouse.KeyUpped += CaptureKeyBoardAndMouse_KeyUpped;

	}
	private static void CaptureKeyBoardAndMouse_KeyDowned(object? sender, KeyEventArgs e)
	{
		KeyDowned(e.KeyCode);
	}
	private static void CaptureKeyBoardAndMouse_KeyUpped(object? sender, KeyEventArgs e)
	{
		KeyUpped(e.KeyCode);
	}


	private static void KeyDowned(Keys key)
	{
		try
		{
			if (key == Keys.LButton)
				LeftMouseClicked?.Invoke();


			if (_presetKeyCombination.Contains(key))
			{
				// Don't add it again if it's already there. This may happen when a user holds down a key.
				if (!PressedKeys.Contains(key))
					PressedKeys.Add(key);			

				if (PressedKeys.Count == _presetKeyCombination.Count)
					CheckHotKeyCombo();
			}
		}
		catch (Exception ex)
		{
			//Logs.Logger.Log(ex);
		}
	}

	private static void KeyUpped(Keys key)
	{
		try
		{
			if (_presetKeyCombination.Contains(key))
			{
				PressedKeys.Remove(key);				
			}
		}
		catch (Exception ex)
		{
			//Logs.Logger.Log(ex);
		}
	}


	private static void CheckHotKeyCombo()
	{
		try
		{
			bool allPressed = true;
			foreach (Keys key in _presetKeyCombination)
			{
				if (!PressedKeys.Contains(key))
					allPressed = false;
			}
			if (allPressed)
			{				
				PressedKeys.Clear();
				KeyCombinationHit?.Invoke();
			}
		}
		catch (Exception ex)
		{
			//Logs.Logger.Log(ex);
		}
	}

	public delegate void KeyCombinationHitEventHandler();
	public static event KeyCombinationHitEventHandler? KeyCombinationHit;

	public delegate void LeftMouseClickEventHandler();
	public static event LeftMouseClickEventHandler? LeftMouseClicked;
}
