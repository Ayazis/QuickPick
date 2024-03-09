using Ayazis.KeyHooks;
using Ayazis.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuickPick.Services;
using QuickPick.UI.Views.Settings;
using QuickPick.Utilities.VirtualDesktop;
using Utilities.Mouse_and_Keyboard;


namespace QuickPick;
public interface IServicesConfig
{
    IHostBuilder CreateHostBuilder();
}
public class ServicesConfig : IServicesConfig
{
    public IHostBuilder CreateHostBuilder()
    {
        return
        Host.CreateDefaultBuilder()
        .ConfigureServices((_, services) =>
            {
                services.AddSingleton<ILogger, FileLogger>();
                services.AddSingleton<IGlobalExceptions, GlobalExceptions>();
                services.AddSingleton<ITrayIconService, TrayIconService>();
                services.AddSingleton<IDesktopTracker, DesktopTracker>();
                services.AddSingleton<IClickWindow, ClickWindow>();
                services.AddSingleton<IMouseAndKeysCapture, MouseAndKeysCapture>();
                services.AddSingleton<IKeyInputHandler, KeyInputHandler>();
                services.AddSingleton<ISettingsManager, SettingsManager>();
                services.AddSingleton<ISettingsWindow, SettingsWindow>();
                services.AddSingleton<IStartup, Startup>();
            }
        );
    }

}
