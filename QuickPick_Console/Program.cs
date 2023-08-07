// See https://aka.ms/new-console-template for more information
using System.Windows.Forms;
using Utilities.Utilities.VirtualDesktop;
using Utilities.VirtualDesktop;
using Ayazis.Utilities;
using Ayazis.KeyHooks;
using Utilities.Mouse_and_Keyboard;
using System.Diagnostics;
using QuickPick.UI.Views;
using QuickPick.Utilities.File_Explorer;

namespace QuickPick;

public class Program
{
    static TrayIconManager _trayIconManager = new TrayIconManager();
    static DesktopTracker _desktopTracker;
    static VirtualDesktopHelper _virtualDesktopHelper = new VirtualDesktopHelper();
    static ClickWindow _clickwindow = new ClickWindow();
    static MouseAndKeysCapture _inputCapture;
    static KeyInputHandler _keyInputHandler;
    static IntPtr _quickPickMainWindowHandle;

    [STAThread]
    static void Main(string[] args)
    {
        try
        {

            var fm = new FileManager();
            var drives = fm.GetLocalDrives();
            var subs = fm.GetChildNodes(drives.First());

            return;
            _trayIconManager.CreateTrayIcon();

            // Setup the ActiveWindows class, this class handles everything related to Open Application Windows.
            ActiveWindows.SetVirtualDesktopHelper(_virtualDesktopHelper);

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
        _desktopTracker = new DesktopTracker(_virtualDesktopHelper);
        _desktopTracker.DesktopChanged += _desktopTracker_DesktopChanged;
        _desktopTracker.StartTracking();
    }

    private static void _keyInputHandler_KeyCombinationHit()
    {
        _clickwindow.ShowWindow();
    }

    static void _desktopTracker_DesktopChanged(object? sender, EventArgs e)
    {
        _clickwindow.UpdateTaskbarShortCuts();

    }
    static void CurrentDomain_ProcessExit(object? sender, EventArgs e)
    {
        _desktopTracker.Dispose();
        _virtualDesktopHelper?.Dispose();
        _trayIconManager.RemoveTrayIcon();
    }
}