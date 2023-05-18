using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using Utilities.VirtualDesktop;
using WindowsDesktop;

namespace QuickPick.PinnedApps;

public class TaskBarApp
{
    public TaskBarApp()
    {
        ClickCommand = new RelayCommand(parameter => AppClicked(this));
    }
    public string Name { get; set; }
    public string TargetPath { get; set; }
    public ImageSource AppIcon { get; set; }
    public ICommand ClickCommand { get; set; }
    public string Arguments { get; set; }
    public bool HasWindowActiveOnCurrentDesktop { get; set; }

    public static void AppClicked(TaskBarApp appInfo)
    {
        var windowHandle = WindowActivator.GetActiveWindow(appInfo.TargetPath);
        if(windowHandle != default)                    
            WindowActivator.ActivateWindow(windowHandle);   
        else
            Task.Run(() => { Process.Start(appInfo.TargetPath, appInfo.Arguments); });
        //WindowActivator.ActivateWindowOnCurrentVirtualDesktop(appInfo.TargetPath, appInfo.Arguments);
        ClickWindow.HideWindow();
    }

    /// <summary>
    /// Checks if there is an active window on the CurrentDeskTop
    /// </summary>
    public void UpdateWindowStatus()
    {
//        WindowActivator.ActivateWindowOnCurrentVirtualDesktop
    }
}


public class RelayCommand : ICommand
{
    private readonly Action<object> _execute;
    private readonly Predicate<object> _canExecute;

    public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public bool CanExecute(object parameter)
    {
        return _canExecute == null || _canExecute(parameter);
    }

    public void Execute(object parameter)
    {
        _execute(parameter);
    }

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}
