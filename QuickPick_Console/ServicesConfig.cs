using Ayazis.KeyHooks;
using Ayazis.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuickPick.Services;
using QuickPick.UI.BrightnessControls;
using QuickPick.UI.Views.Settings;
using QuickPick.Utilities.VirtualDesktop;
using Utilities.Mouse_and_Keyboard;

namespace QuickPick;

public static class ServicesConfig
{
    public static IHost AppHost;
    static ServicesConfig()
    {
        AppHost = CreateHostBuilder().Build();
    }

    private static IHostBuilder CreateHostBuilder()
    {
        return
        Host.CreateDefaultBuilder()
        .ConfigureServices((_, services) =>
            {
                services.AddSingleton<ILogger, FileLogger>();
                services.AddSingleton<IGlobalExceptions, GlobalExceptions>();
                services.AddSingleton<ITrayIconService, TrayIconService>();
                services.AddSingleton<IDesktopTracker, DesktopTracker>();
                services.AddSingleton<IMouseAndKeysCapture, MouseAndKeysCapture>();
                services.AddSingleton<IKeyInputHandler, KeyInputHandler>();
                services.AddSingleton(new SettingsViewModel());
                services.AddSingleton<ISettingsSaver, SettingsSaver>();
                services.AddSingleton<ISettingsWindow, SettingsWindow>();
                services.AddSingleton<IClickWindow, ClickWindow>();
                services.AddSingleton<IStartup, Startup>();
            }
        );
    }
}