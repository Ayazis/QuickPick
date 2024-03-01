using Ayazis.KeyHooks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuickPick.Services;
using QuickPick.Utilities.VirtualDesktop;


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
                services.AddSingleton<IGlobalExceptions, GlobalExceptions>();
                services.AddSingleton<ITrayIconService, TrayIconService>();
                services.AddSingleton<IDesktopTracker, DesktopTracker>();
                services.AddSingleton<IClickWindow, ClickWindow>();
                services.AddSingleton<IMouseAndKeysCapture, MouseAndKeysCapture>();
                services.AddSingleton<IStartup, Startup>();
            }
        );
    }

}
