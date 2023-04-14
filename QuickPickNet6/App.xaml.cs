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

namespace QuickPickNet6;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);

		// Set Keyboard hooks
		List<Keys> keyCombination = new List<Keys> { Keys.LMenu, Keys.RButton };


		HotKeys.SubscribeToKeyEvents(keyCombination);

		// Perform any initialization tasks here
		// For example, you can create objects and set properties, or show a splash screen

		// Create the main window and show it
		var mainWindow = new MainWindow();
		mainWindow.Show();


		// Set Mainwindow Handle (needed for displaying thumbnails)

	}
}
