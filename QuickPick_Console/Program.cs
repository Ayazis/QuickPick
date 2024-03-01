using Ayazis.KeyHooks;
using Ayazis.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuickPick.UI.Views.Settings;
using System.Windows;
using System.Windows.Threading;
using Utilities.Mouse_and_Keyboard;
using Utilities.VirtualDesktop;

namespace QuickPick;

public class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        try
        {
            IHost host = new ServicesConfig().CreateHostBuilder().Build();
            IStartup startup = host.Services.GetRequiredService<IStartup>();          

            SettingsManager.Instance.LoadSettings();
            SettingsWindow.Instance.ViewModel.ApplySettings(SettingsManager.Instance.Settings);           

            startup.StartApplication();        

        }
        catch (Exception ex)
        {
            Logs.Logger?.Log(ex);
        }
    } 
}