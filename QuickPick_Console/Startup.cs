using Ayazis.KeyHooks;
using QuickPick.Services;
using System.Windows;
using System.Windows.Threading;
using Utilities.Mouse_and_Keyboard;
using QuickPick.Utilities.VirtualDesktop;

namespace QuickPick;

internal interface IStartup
{
    void StartApplication();
}

internal class Startup : IStartup
{
    IGlobalExceptions _globalExceptions;
    ITrayIconService _trayIconService;
    IDesktopTracker _desktopTracker;
    IClickWindow _clickWindow;
    IMouseAndKeysCapture _mouseAndKeysCapture;
    public Startup(IGlobalExceptions globalExceptions,
        ITrayIconService trayIconService,
        IClickWindow clickWindow,
        IDesktopTracker desktopTracker,
        IMouseAndKeysCapture mouseAndKeysCapture)
    {
        _globalExceptions = globalExceptions;
        _trayIconService = trayIconService;
        _clickWindow = clickWindow;
        _desktopTracker = desktopTracker;
        _mouseAndKeysCapture = mouseAndKeysCapture;
    }
    public void StartApplication()
    {
        _globalExceptions.SetupGlobalExceptionHandling();
        _trayIconService.CreateTrayIcon();

        StartDesktopTracking();
        SubscribeToExitEventToHandleCleanup();
        StartListeningToKeyboardAndMouse();

        // Run the application indefinetely.
        new Application().Run();
    }

    private void StartListeningToKeyboardAndMouse()
    {
        KeyInputHandler.Instance.SetKeycombination(SettingsManager.Instance.Settings.KeyCombination);

        _mouseAndKeysCapture.HookIntoMouseAndKeyBoard();

        KeyInputHandler.Instance.KeyCombinationHit += _keyInputHandler_KeyCombinationHit;
    }
    private void _keyInputHandler_KeyCombinationHit()
    {

        Task.Run(() => { _clickWindow.UpdateTaskbarShortCuts(); });


        DispatcherTimer timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromMilliseconds(100);

        // Show the window
        ClickWindow.Instance.ShowWindow();

        // Some actions need to be done slightly later then the ShowWindown() method, when the UI is showing. 
        // Using a Timer we make sure the UI is loaded before performing these actions.
        timer.Tick += (sender, e) =>
        {
            ClickWindow.Instance.Activate(); // Set focus to the Window after it is shown. If not done, the deactivated event will not fire.

            // Set the deactivated event after the is shown, otherwise the UI will hide instantly.
            ClickWindow.Instance.Deactivated += ClickWindow.Instance.HandleFocusLost;
            ClickWindow.Instance.LostFocus += ClickWindow.Instance.HandleFocusLost;

            // Stop the timer, as we only want this to happen once
            timer.Stop();
        };

        // Start the timer
        timer.Start();

    }

    private void StartDesktopTracking()
    {
        // Todo: see if the desktop interface is compatible with current .dll.

        // On every desktop change, the current active windows for that desktop are retrieved.
        _desktopTracker.DesktopChanged += (sender, e) => _clickWindow.UpdateTaskbarShortCuts();
        _desktopTracker.StartTracking();
    }

    private void SubscribeToExitEventToHandleCleanup()
    {
        AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
    }

     void CurrentDomain_ProcessExit(object? sender, EventArgs e)
    {
        _desktopTracker?.Dispose();
        _trayIconService.RemoveTrayIcon();
    }
}
