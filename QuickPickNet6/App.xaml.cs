using Ayazis.KeyHooks;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Application = System.Windows.Application;
using System.Windows.Forms;
using System.Windows;
using System.Diagnostics;

namespace QuickPickNet6;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
	MainWindow _mainWindow;
	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);

		// Set Keyboard hooks
		List<Keys> keyCombination = new List<Keys> { Keys.LMenu, Keys.RButton };
		HotKeys.SubscribeToKeyEvents(keyCombination);

		// Two main events that need handling for UI purposes.
		// The logic for these should not remain within this class.
		HotKeys.KeyCombinationHit += HotKeys_KeyCombinationHit;
		HotKeys.LeftMouseClicked += HotKeys_LeftMouseClicked;	
				
		_mainWindow = new MainWindow();

		var mainHandle = SetQuickPicksMainWindowHandle();

		new TrayIcon().CreateTrayIcon();

	}

	private IntPtr SetQuickPicksMainWindowHandle()
	{		
		// Getting the window handle only works when the app is shown in the taskbar & the mainwindow is shown.
		// The handle remains usable after setting this to false.
		_mainWindow.Show();
		_mainWindow.ShowInTaskbar = true;
		Process currentProcess = Process.GetCurrentProcess();
		var quickPickMainWindowHandle = currentProcess.MainWindowHandle;
		_mainWindow.ShowInTaskbar = false;
		_mainWindow.Hide();
		return quickPickMainWindowHandle;		
	}

	private void HotKeys_LeftMouseClicked()
	{
		Debug.WriteLine("Left mouse!");
	}

	private void HotKeys_KeyCombinationHit()
	{
		Debug.WriteLine("KeyCombo!");
	}


}
