// See https://aka.ms/new-console-template for more information
using QuickPick;
using System.Windows.Forms;
using Utilities.Utilities.VirtualDesktop;
using Utilities.VirtualDesktop;

Console.WriteLine("Hello, World!");


TrayIconManager _trayIconManager = new TrayIconManager();
DesktopTracker _desktopTracker;
VirtualDesktopHelper _VirtualDesktopHelper;


_trayIconManager.CreateTrayIcon();

_VirtualDesktopHelper = new VirtualDesktopHelper();
ActiveWindows.Initialise(_VirtualDesktopHelper);
_desktopTracker = new DesktopTracker(_VirtualDesktopHelper);
_desktopTracker.StartTracking();
AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;


using (var context = new ApplicationContext())
{
	System.Windows.Forms.Application.Run(context);
}

void CurrentDomain_ProcessExit(object? sender, EventArgs e)
{
	_VirtualDesktopHelper?.Dispose();
}

