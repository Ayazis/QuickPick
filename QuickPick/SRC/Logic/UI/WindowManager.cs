using QuickPick.Classes;
using QuickPick.Logic;
using QuickPick.Models;
using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Animation;
using QuickPick.SRC.Logic;
using ThumbnailLogic;
using System.Diagnostics;

namespace QuickPick
{
	public class WindowManager
	{
		public Models.QuickPick QP { get; set; }
		private NotifyIcon _notificationIcon;
		IntPtr _ActiveWindowHandle; // used to store the current application whenever quickPick is activated.

		public ClickWindow ClickWindow { get; set; }
		private SettingsWindow _settingsWindow;
		IntPtr _quickPickMainWindowHandle;

		public Storyboard Hide { get; private set; }
		public Storyboard Show { get; private set; }

		[DllImport("user32.dll")]
		static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll", SetLastError = true)]
		static extern bool SetForegroundWindow(IntPtr hWnd);


		public WindowManager(Models.QuickPick quickPick)
		{
			QP = quickPick;
		}

		public void Start()
		{
			CreateTrayIcon();
			CreateWindow();
			FindResources();
			this.ClickWindow.ShowInTaskbar = false;

		}

		private void FindResources()
		{
			this.Hide = ClickWindow.TryFindResource("hideMe") as Storyboard;
			this.Show = ClickWindow.TryFindResource("showMe") as Storyboard;
		}

		private void CreateTrayIcon()
		{
			// CREATE TRAYICON
			var menu = new ContextMenu();
			var mnuExit = new MenuItem("Exit");
			var mnuSettings = new MenuItem("Settings");
			menu.MenuItems.Add(0, mnuExit);
			menu.MenuItems.Add(0, mnuSettings);

			_notificationIcon = new NotifyIcon()
			{
				Icon = CreateIcon(),
				//Icon = new Icon(SystemIcons.Warning, 40, 40),
				ContextMenu = menu,
				Text = "Main"
			};

			mnuExit.Click += new EventHandler(mnuExit_Click);
			mnuSettings.Click += MnuSettings_Click;
			_notificationIcon.Visible = true;
		}

		private Icon CreateIcon()
		{
			var currentPath = AppDomain.CurrentDomain.BaseDirectory;
			var IconPath = $@"{currentPath}\SRC\Assets\QP_Icon_32px.png";

			var bitmap = new Bitmap(IconPath);
			var iconHandle = bitmap.GetHicon();

			return Icon.FromHandle(iconHandle);
		}


		private void MnuSettings_Click(object sender, EventArgs e)
		{
			try
			{
				_settingsWindow = new SettingsWindow(QP);
				_settingsWindow.WindowStyle = WindowStyle.None;
				_settingsWindow.DataContext = QP.QuickPickModel;
				_settingsWindow.Show();

			}
			catch (Exception ex)
			{
				Logs.Logger.Log(ex);
			}
		}


		private void CreateWindow()
		{
			try
			{
				ClickWindow = new ClickWindow(QP);
				QP.SaveLoader.LoadSettingsFile();

				ClickWindow.WindowStartupLocation = WindowStartupLocation.Manual;
				ClickWindow.WindowStyle = WindowStyle.None;
				ClickWindow.Topmost = true;
				ClickWindow.Show();

				SetQuickPicksMainWindowHandle();

				ClickWindow.Visibility = Visibility.Hidden;
				ClickWindow.Closed += Window_Closed;
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
			ClickWindow.ShowInTaskbar = true;
			Process currentProcess = Process.GetCurrentProcess();
			_quickPickMainWindowHandle = currentProcess.MainWindowHandle;
			ClickWindow.ShowInTaskbar = false;

		}

		private void Window_Closed(object sender, EventArgs e)
		{
			_notificationIcon.Dispose();
		}

		public bool MouseIsOutsideWindow()
		{
			var mouse = GetMousePosition();

			bool isOutside = (mouse.X < ClickWindow.Left || mouse.X > ClickWindow.Left + ClickWindow.ActualWidth)
							|| (mouse.Y < ClickWindow.Top || mouse.Y > ClickWindow.Top + ClickWindow.ActualHeight);

			return isOutside;
		}

		public void ShowWindow()
		{
			try
			{
				SetActiveWindow();

				ClickWindow.Dispatcher.Invoke(() =>
				{
					HideShortCuts();
					var mousePosition = GetMousePosition();
					ClickWindow.Left = mousePosition.X - (ClickWindow.ActualWidth / 2);
					ClickWindow.Top = mousePosition.Y - (ClickWindow.ActualHeight / 2);

					ClickWindow.WindowStyle = WindowStyle.None;
					Show.Begin(ClickWindow);
				});

				if (QP.QuickPickModel.InstantShortCuts)
				{
					ShowShortCuts();
				}

				ShowPreviews();

			}
			catch (Exception ex)
			{
				Logs.Logger.Log(ex);
			}
		}

		private void ShowPreviews()
		{
			var allOpenWindows = ActiveApps.GetAllOpenWindows();

			double sizeFactor = 0.1;

			double x = 0;
			double y = 0;
			double xmax = 1920 * sizeFactor;
			double ymax = 1080 * sizeFactor;
			foreach (var process in allOpenWindows)
			{
				var thumbHandle = Thumbnails.GetThumbnailRelations(process.MainWindowHandle, _quickPickMainWindowHandle);
				if (thumbHandle == default)
					continue;
				RECT rect = new RECT((int)x, (int)y, (int)xmax, (int)ymax);
				Thumbnails.CreateThumbnail(thumbHandle, rect);
				x += xmax;
				xmax += xmax;
			}
		}

		private System.Drawing.Point GetMousePosition()
		{
			return MousePosition.GetCursorPosition();
		}

		public void ReActivateFormerWindow()
		{
			try
			{
				SetForegroundWindow(_ActiveWindowHandle);
			}
			catch (Exception)
			{
				// Do nothing, except log..?
			}
		}

		public void SetActiveWindow()
		{
			try
			{
				const int nChars = 256;
				StringBuilder Buff = new StringBuilder(nChars);
				_ActiveWindowHandle = GetForegroundWindow();
			}
			catch (Exception ex)
			{
				Logs.Logger.Log(ex);
			}
		}

		private void mnuExit_Click(object sender, EventArgs e)
		{
			_notificationIcon.Visible = false;
			_notificationIcon.Dispose();
			System.Windows.Forms.Application.Exit();
		}

		public void ShowShortCuts()
		{
			foreach (var b in QP.QuickPickModel.ShortCutButtons)
			{
				b.Icon.Visibility = Visibility.Visible;
			}
		}
		public void HideShortCuts()
		{
			foreach (var b in QP.QuickPickModel.ShortCutButtons)
			{
				b.Icon.Visibility = Visibility.Hidden;
			}
		}

	}
}
