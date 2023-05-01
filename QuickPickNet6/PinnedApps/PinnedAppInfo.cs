using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System;

namespace QuickPick.PinnedApps;

public class PinnedAppInfo
{
    public string Name { get; set; }
    public string TargetPath { get; set; }
    public ImageSource AppIcon { get; set; }
    public ICommand ClickCommand { get; set; }

    public static void AppClicked(object parameter)
    {
        if (parameter is PinnedAppInfo app)
        {
            // Handle app click logic here, e.g., launch the app using app.TargetPath
        }
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
