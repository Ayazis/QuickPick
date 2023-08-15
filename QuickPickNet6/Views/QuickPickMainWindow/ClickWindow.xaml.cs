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
using ThumbnailLogic;
using System.Linq;
using System.Windows.Input;
using Microsoft.VisualBasic;
using QuickPick.UI.Views.Thumbnail;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace QuickPick;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class ClickWindow : Window
{
	private static ClickWindow _instance;
	private QuickPickMainWindowModel _qpm = new QuickPickMainWindowModel();
	private IntPtr _quickPickWindowHandle;
	private List<ThumbnailView> _currentThumbnails = new List<ThumbnailView>();

	public Storyboard HideAnimation { get; private set; }
	public Storyboard ShowAnimation { get; private set; }
	public ClickWindow()
	{
		InitializeComponent();
		this.PreviewMouseWheel += ClickWindow_PreviewMouseWheel; ;
		DataContext = _qpm;

		HideAnimation = TryFindResource("hideMe") as Storyboard;
		ShowAnimation = TryFindResource("showMe") as Storyboard;


		SetQuickPicksMainWindowHandle();
		UpdateLayout();
		_instance = this;
	}

	private void ClickWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		if (MouseIsOutsideWindow())
			HideAnimation.Begin(this);
	}

	private void ClickWindow_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
	{
		var collection = _qpm.PinnedApps;
		if (e.Delta > 0)
		{       // Scrolled up
			if (collection.Count > 1)
			{
				collection.Move(0, collection.Count - 1);
				//AppLink firstItem = collection[0];
				//collection.RemoveAt(0);
				//collection.Add(firstItem);
			}
		}
		else
		{
			// Scrolled down
			// Scrolled down
			if (collection.Count > 1)
			{
				collection.Move(collection.Count - 1, 0);
			}
		}
	}

	public bool MouseIsOutsideWindow()
	{
		var mouse = MousePosition.GetCursorPosition();

		bool isOutside = mouse.X < Left || mouse.X > Left + ActualWidth
						|| mouse.Y < Top || mouse.Y > Top + ActualHeight;

		return isOutside;
	}

	public void UpdateTaskbarShortCuts()
	{
		List<AppLink> apps = AppLinkRetriever.GetPinnedAppsAndActiveWindows();

		foreach (var app in apps)
		{
			var handle = ActiveWindows.GetActiveWindowOnCurentDesktop(app.TargetPath);
			if (handle != default)
				app.HasWindowActiveOnCurrentDesktop = true;
		}


		// Group the apps by filePath
		var grouped = apps.GroupBy(g => g.TargetPath);
		foreach (var group in grouped)
		{
			// Foreach 
			var openWindows = group.ToList();
			var main = openWindows.First();
			main.WindowHandles = openWindows.SelectMany(s => s.WindowHandles).Distinct().ToList();
		}


		_qpm.PinnedApps = new ObservableCollection<AppLink>(apps);
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
			Visibility = Visibility.Visible;
			var mousePosition = MousePosition.GetCursorPosition();
			Left = mousePosition.X - ActualWidth / 2;
			Top = mousePosition.Y - ActualHeight / 2;
			ShowAnimation.Begin(this);

		}
		catch (Exception ex)
		{
			Logs.Logger.Log(ex);
		}
	}

	private void SetQuickPicksMainWindowHandle()
	{
		// Getting the window handle only works when the app is shown in the taskbar.
		// hHe handle remains usable after setting this to false.
		ShowInTaskbar = true;
		Show();
		Process currentProcess = Process.GetCurrentProcess();
		_quickPickWindowHandle = currentProcess.MainWindowHandle;
		ShowInTaskbar = false;
		Hide();

	}
	private void Button_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
	{
		// todo: Move logic out of xaml.xs
		var button = (System.Windows.Controls.Button)sender;
		AppLink pinnedApp = button.DataContext as AppLink;	

		CreateThumbnails(pinnedApp, button);
	}

	private void CreateThumbnails(AppLink pinnedApp, System.Windows.Controls.Button button)
	{
		// Get the center of the button relative to its container (the window)
		var buttonCenter = button.TransformToAncestor(this)
								.Transform(new Point(button.ActualWidth / 2, button.ActualHeight / 2));

		// Get the center of the window
		double windowCenterX = this.ActualWidth / 2;
		double windowCenterY = this.ActualHeight / 2;

		// Calculate the button's position relative to the window's center
		double xToCenter = buttonCenter.X - windowCenterX;
		double ytoCenter = buttonCenter.Y - windowCenterY;

		// Get DPI information
		PresentationSource source = PresentationSource.FromVisual(this);
		var m = source.CompositionTarget.TransformToDevice;
		double dpiScaling = m.M11;

		for (int i = 0; i < pinnedApp.WindowHandles.Count; i++)
		{
			IntPtr currentWindowHandle = pinnedApp.WindowHandles[i];

			// Create thumbnailRelation
			var newThumbnail = ThumbnailCreator.GetThumbnailRelations(currentWindowHandle, _quickPickWindowHandle);
			if (newThumbnail == default)
				continue;

			RECT rect = CreateRectForThumbnail(buttonCenter, xToCenter, ytoCenter, dpiScaling, i, currentWindowHandle);

			var thumbnailContext = new ThumbnailDataContext(newThumbnail, rect);
			var thumbnailView = new ThumbnailView(thumbnailContext);

			this.ThumbnailCanvas.Children.Add(thumbnailView);
			_currentThumbnails.Add(thumbnailView);

			// Set the position of ThumbnailView based on translated coordinates.
			Canvas.SetLeft(thumbnailView, rect.Left / dpiScaling);
			Canvas.SetTop(thumbnailView, rect.Top / dpiScaling - 20);
			thumbnailView.FadeIn();
		};
	}

	private static RECT CreateRectForThumbnail(Point buttonCenter, double xToCenter, double ytoCenter, double dpiScaling, int i, IntPtr currentWindowHandle)
	{
		double aspectRatio = ThumbnailCreator.GetWindowAspectRatio(currentWindowHandle);

		double height, width;

		bool isLandscape = aspectRatio > 1;
		if (isLandscape)
		{
			width = 200;
			height = width / aspectRatio;
		}
		else
		{
			height = 200;
			width = height * aspectRatio;
		}

		double thumbnailX;

		thumbnailX = buttonCenter.X + xToCenter;
		bool isLeftToCenter = xToCenter < 0;

		// make room for the thumbnail.
		if (isLeftToCenter)
			thumbnailX -= width;

		// align the vertical center of the thumbnail
		double thumbnailY = buttonCenter.Y + ytoCenter - height / 2;

		int left = (int)(thumbnailX * dpiScaling + (i * width));
		int top = (int)(thumbnailY * dpiScaling);
		int right = (int)((thumbnailX + width) * dpiScaling + (i * width));
		int bottom = (int)((thumbnailY + height) * dpiScaling);

		RECT rect = new RECT(left, top, right, bottom);
		return rect;
	}

	private void Button_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
	{
		foreach (var thumbnailView in _currentThumbnails)
		{
			if (thumbnailView != default)
				thumbnailView.Hide();
		}
		_currentThumbnails.Clear();
	}
}
