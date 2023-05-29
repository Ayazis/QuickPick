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
using System.Diagnostics;
using System.Collections.ObjectModel;
using QuickPick.PinnedApps;
using System.ComponentModel;
using Utilities.Utilities.VirtualDesktop;
using Utilities.VirtualDesktop;

namespace QuickPick;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    TrayIconManager _trayIconManager = new TrayIconManager();
    DesktopTracker _desktopTracker;
    VirtualDesktopHelper _VirtualDesktopHelper;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        _trayIconManager.CreateTrayIcon();

        _VirtualDesktopHelper = new VirtualDesktopHelper();
        ActiveWindows.Initialise(_VirtualDesktopHelper);
        _desktopTracker = new DesktopTracker(_VirtualDesktopHelper);
        _desktopTracker.DesktopChanged += _virtualDesktopManager_DesktopChanged;
        _desktopTracker.StartTracking();
        AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;


    }

    private void CurrentDomain_ProcessExit(object sender, EventArgs e)
    {
        ExitApplication();
    }

    private void _virtualDesktopManager_DesktopChanged(object sender, EventArgs e)
    {
        // Update the tasks.
        ClickWindow.UpdateTaskbarShortCuts();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        //base.OnExit(e);
       // ExitApplication();
    }

    private void ExitApplication()
    {
        _trayIconManager?.RemoveTrayIcon();
        _VirtualDesktopHelper?.Dispose();
    }

}
