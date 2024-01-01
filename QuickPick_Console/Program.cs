using Ayazis.KeyHooks;
using Ayazis.Utilities;
using QuickPick.UI.Views.Settings;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using UpdateInstaller;
using UpdateInstaller.Updates;
using Updates;
using Utilities.Mouse_and_Keyboard;
using Utilities.VirtualDesktop;

namespace QuickPick;

public class Program
{
    static TrayIconManager _trayIconManager = new TrayIconManager();
    static DesktopTracker _desktopTracker;
    static ClickWindow _clickwindow = new ClickWindow();
    static MouseAndKeysCapture _inputCapture;

    [STAThread]
    static void Main(string[] args)
    {


        try
        {
            SettingsManager.Instance.LoadSettings();
            SettingsWindow.Instance.ViewModel.ApplySettings(SettingsManager.Instance.Settings);
            _trayIconManager.CreateTrayIcon();
#if !DEBUG
			//CheckInputArguments(args);
#else
            //CheckInputArguments(args);
#endif
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

    private static void CheckInputArguments(string[] args)
    {
        InstallerArguments? arguments = TryGetInstallerArguments(args);
        if (arguments == null)
        {
            UpdateIfAvailable();
        }
        else
        {
            UpdateAndRestart((InstallerArguments)arguments);
        }
    }

    private static void UpdateAndRestart(InstallerArguments arguments)
    {
        new ApplicationCloser().CloseApplication(arguments.ProcessIdToKill);
        FileCopier.CopyFiles(arguments.SourceFolder, arguments.TargetFolder);
        Process.Start(arguments.PathToExecutable, arguments.TargetArguments);
        Application.Current.Shutdown();
    }

    private static void UpdateIfAvailable()
    {
        // check for update
        GithubUpdateChecker updateChecker = new GithubUpdateChecker("Ayazis", "QuickPick");
        UpdateDownloader updateDownloader = new UpdateDownloader(eUpdateType.Pre_Release, updateChecker);
        UpdateDownloadManager updateManager = new UpdateDownloadManager(updateDownloader);
        bool UpdateIsAvailable = updateManager.CheckIfUpdateIsAvailableAsync().Result;
        if (UpdateIsAvailable)
        {
            //new StartWindow().Show();
            InstallerParams installerParams = updateManager.DownloadUpdateAndGetInstallerArguments();
            Process.Start(installerParams.InstallerPath, installerParams.Arguments.ToStringArray());
            Application.Current.Shutdown();
        }
    }

    static InstallerArguments? TryGetInstallerArguments(string[] args)
    {
        if (args == null || args.Length == 0)
            return null;

        try
        {
            return InstallerArguments.FromStringArray(args);
        }
        catch (Exception)
        {
            return null;
        }
    }

    private static void RunApplicationIndefinetely()
    {  // Create the Application object
        Application app = new();
        app.Run();
    }

    private static void SubscribeToExitEvent_ToHandleCleanup()
    {
        AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
    }

    private static void StartListeningToKeyboardAndMouse()
    {
        KeyInputHandler.Instance.SetKeycombination(SettingsManager.Instance.Settings.KeyCombination);
        _inputCapture = new MouseAndKeysCapture();
        _inputCapture.HookIntoMouseAndKeyBoard();

        KeyInputHandler.Instance.KeyCombinationHit += _keyInputHandler_KeyCombinationHit;
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