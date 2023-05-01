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
using System.Collections.ObjectModel;
using QuickPick.PinnedApps;

namespace QuickPick;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
	ClickWindow clickWindow;
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
        
		new TrayIconManager().CreateTrayIcon();		
	}

	private void HotKeys_LeftMouseClicked()
	{
		//Debug.WriteLine("Left mouse!");
	}

	private void HotKeys_KeyCombinationHit()
	{
		//Debug.WriteLine("KeyCombo!");
	}


}
