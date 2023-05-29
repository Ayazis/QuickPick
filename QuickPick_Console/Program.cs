// See https://aka.ms/new-console-template for more information
using QuickPick;
using System.Windows.Forms;
using Utilities.Utilities.VirtualDesktop;
using Utilities.VirtualDesktop;
using QuickPick.Models;


using System.Drawing;
using Ayazis.Utilities;
using Ayazis.KeyHooks;
using Utilities.Mouse_and_Keyboard;

namespace QuickPick
{
	public class Program
	{
		static TrayIconManager _trayIconManager = new TrayIconManager();
		static DesktopTracker _desktopTracker;
		static VirtualDesktopHelper _VirtualDesktopHelper;
		static ClickWindow _clickwindows;
		private static MouseAndKeysCapture _inputCapture;
		static KeyInputHandler _keyInputHandler;

		[STAThread]
		static void Main(string[] args)
		{
			try
			{
				_clickwindows = new ClickWindow();
				_trayIconManager.CreateTrayIcon();

				_VirtualDesktopHelper = new VirtualDesktopHelper();
				ActiveWindows.Initialise(_VirtualDesktopHelper);
				_desktopTracker = new DesktopTracker(_VirtualDesktopHelper);
				_desktopTracker.DesktopChanged += _desktopTracker_DesktopChanged;
				_desktopTracker.StartTracking();



				List<Keys> keyCombination = new List<Keys> { Keys.LMenu, Keys.RButton };
				_keyInputHandler = new KeyInputHandler(keyCombination);
				_inputCapture = new MouseAndKeysCapture(_keyInputHandler);
				_inputCapture.HookIntoMouseAndKeyBoard();

				_keyInputHandler.KeyCombinationHit += _keyInputHandler_KeyCombinationHit;

				AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;




				using (var context = new ApplicationContext())
				{
					Application.Run(context);
				}


			}
			catch (Exception ex)
			{
				Logs.Logger?.Log(ex);
			}
		}

		private static void _keyInputHandler_KeyCombinationHit()
		{
			_clickwindows.OnKeyCombinationHit();
		}

		static void _desktopTracker_DesktopChanged(object? sender, EventArgs e)
		{
			_clickwindows.UpdateTaskbarShortCuts();

			

		}
		static void CurrentDomain_ProcessExit(object? sender, EventArgs e)
		{
			_VirtualDesktopHelper?.Dispose();
		}
	}

}