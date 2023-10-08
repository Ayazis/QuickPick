using System.Windows.Forms;
using Utilities.VirtualDesktop;
using Ayazis.Utilities;
using Ayazis.KeyHooks;
using Utilities.Mouse_and_Keyboard;
using QuickPick.UI;
using Updates;
using UpdateInstaller;
using UpdateInstaller.Updates;
using System.Diagnostics;
using System.Reflection;

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
            var updateChecker = new GithubUpdateChecker("Ayazis", "QuickPick");
            Assembly assembly = Assembly.GetExecutingAssembly();
            Version currentVersion = assembly.GetName().Version;
#if DEBUG
            currentVersion = new Version("0.0.0");
#endif

            bool updateAvailable = updateChecker.IsUpdateAvailableAsync(eUpdateType.Pre_Release, currentVersion).Result;

            if (updateAvailable)
            {
                // todo: decouple updater from window
                // create updater and subscribe windowupdate to event
                // show window
                // start update
                // RunApp
                // download done? Run UpdateInstallerApp

                UpdateDownloader updater = new UpdateDownloader(eUpdateType.Pre_Release, updateChecker);
                var updateWindow = new StartWindow();
                updateWindow.Show();

                var downloadedFile = updateWindow.StartDownloadAsync(updater).GetAwaiter().GetResult();

                RunApplicationIndefinetely();

                string pathToCurrentExecutable = Process.GetCurrentProcess().MainModule.FileName;
                string targetDirectory = Path.GetFileNameWithoutExtension(downloadedFile);
                if (!Directory.Exists(targetDirectory))
                    Directory.CreateDirectory(targetDirectory);

                new ArchiveExtractor().ExtractFiles(pathToCurrentExecutable, targetDirectory);

            }



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