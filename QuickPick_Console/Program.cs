using System.Windows.Forms;
using Utilities.VirtualDesktop;
using Ayazis.Utilities;
using Ayazis.KeyHooks;
using Utilities.Mouse_and_Keyboard;
namespace QuickPick;

public class Program
{
	static TrayIconManager _trayIconManager = new TrayIconManager();
	static DesktopTracker _desktopTracker;
	static ClickWindow _clickwindow = new ClickWindow();
	static MouseAndKeysCapture _inputCapture;
	static KeyInputHandler _keyInputHandler;
	static IntPtr _quickPickMainWindowHandle;

	[STAThread]
	static void Main(string[] args)
	{
		try
		{			
			Updater updater = new();			
			bool updateIsAvailable = updater.IsUpdateAvailableAsync().GetAwaiter().GetResult();
			if (updateIsAvailable)
				updater.InstallUpdateAsync().GetAwaiter().GetResult();

			_trayIconManager.CreateTrayIcon();

			// On every desktop change, the current active windows for that desktop are retrieved.
			StartDesktopTracking();

			// Hook into Keyboard and Mouse to listen for User set Keycombination.
			StartListeningToKeyboardAndMouse();

			SubscribeToExitEvent_ToHandleCleanup();

			RunApplicationIndefinetely();

		}
		catch (Exception ex)
		{
			Logs.Logger?.Log(ex);
		}
	}

	private static void RunApplicationIndefinetely()
	{
		using (var context = new ApplicationContext())
		{
			Application.Run(context);
		}
	}

	private static void SubscribeToExitEvent_ToHandleCleanup()
	{
		AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
	}

	private static void StartListeningToKeyboardAndMouse()
	{
		List<Keys> keyCombination = new List<Keys> { Keys.LControlKey, Keys.RButton };
		_keyInputHandler = new KeyInputHandler(keyCombination);
		_inputCapture = new MouseAndKeysCapture(_keyInputHandler);
		_inputCapture.HookIntoMouseAndKeyBoard();

		_keyInputHandler.KeyCombinationHit += _keyInputHandler_KeyCombinationHit;
	}

	private static void StartDesktopTracking()
	{
		_desktopTracker = new DesktopTracker();
		_desktopTracker.DesktopChanged += _desktopTracker_DesktopChanged;
		_desktopTracker.StartTracking();
	}

	private static void _keyInputHandler_KeyCombinationHit()
	{
		Task.Run(() => { _clickwindow.UpdateTaskbarShortCuts(); });
		_clickwindow.ShowWindow();
	}

	static void _desktopTracker_DesktopChanged(object? sender, EventArgs e)
	{
		_clickwindow.UpdateTaskbarShortCuts();

	}
	static void CurrentDomain_ProcessExit(object? sender, EventArgs e)
	{
		_desktopTracker.Dispose();
		_trayIconManager.RemoveTrayIcon();
	}
}