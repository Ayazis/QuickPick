using System;
using System.Collections.Generic;
using System.Windows;
using QuickPick.PinnedApps;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Ayazis.KeyHooks;
using System.Windows.Media.Animation;
using MouseAndKeyBoardHooks;
using Ayazis.Utilities;
using Utilities.Mouse_and_Keyboard;
using System.Windows.Forms;

namespace QuickPick;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class ClickWindow : Window
{
	private static ClickWindow _instance;
	private QuickPickMainWindowModel _qpm = new QuickPickMainWindowModel();
	public Storyboard HideAnimation { get; private set; }
	public Storyboard ShowAnimation { get; private set; }
	public ClickWindow()
	{
		InitializeComponent();		
		DataContext = _qpm;

		HideAnimation = TryFindResource("hideMe") as Storyboard;
		ShowAnimation = TryFindResource("showMe") as Storyboard;

		List<Keys> keyCombination = new List<Keys> { Keys.LMenu, Keys.RButton };
		var keyInputHandler = new KeyInputHandler(keyCombination);
		var inputCapture = new MouseAndKeysCapture(keyInputHandler);
		inputCapture.HookIntoMouseAndKeyBoard();
		keyInputHandler.KeyCombinationHit += OnKeyCombinationHit;
		ShowWindowInvisible();
		_instance = this;
	}



	private void HotKeys_LeftMouseClicked()
	{
		if (MouseIsOutsideWindow())
			HideAnimation.Begin(this);
	}
	public bool MouseIsOutsideWindow()
	{
		var mouse = MousePosition.GetCursorPosition();

		bool isOutside = (mouse.X < this.Left || mouse.X > this.Left + this.ActualWidth)
						|| (mouse.Y < this.Top || mouse.Y > this.Top + this.ActualHeight);

		return isOutside;
	}
	private void ShowWindowInvisible()
	{
		Opacity = 0;
		Show();
		Visibility = Visibility.Hidden;
		Opacity = 1;
	}

	private void OnKeyCombinationHit()
	{				
		ShowWindow();
		UpdateTaskbarShortCut();	
	}

	private void UpdateTaskbarShortCut()
	{
		List<TaskbarShortCut> apps = TaskbarApps.GetPinnedAppsAndActiveWindows();	

		foreach ( var app in apps ) 
		{
			var handle = ActiveWindows.GetActiveWindowOnCurentDesktop(app.TargetPath);
			if (handle != default)
				app.HasWindowActiveOnCurrentDesktop = true;
		}
		_qpm.PinnedApps = new ObservableCollection<TaskbarShortCut>(apps);
		_qpm.NotifyPropertyChanged(nameof(_qpm.PinnedApps));
	}

	public static void HideWindow()
	{
		try
		{
            _instance.HideAnimation.Begin(_instance);            

		}
		catch (Exception ex)
		{
			Logs.Logger.Log(ex);
		}
	}

	public void ShowWindow()
	{
		try
		{
            //SetActiveWindow();
            //HideShortCuts();
            var mousePosition = MousePosition.GetCursorPosition();
            this.Left = mousePosition.X - (this.ActualWidth / 2);
            this.Top = mousePosition.Y - (this.ActualHeight / 2);
            ShowAnimation.Begin(this);       

		}
		catch (Exception ex)
		{
			Logs.Logger.Log(ex);
		}
	}
}
