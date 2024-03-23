using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuickPick.UI.BrightnessControls;
namespace QuickPick;
public class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        new BrightnessControl();
        IHost host = new ServicesConfig()
            .CreateHostBuilder()
            .Build();

        IStartup startup = host.Services.GetRequiredService<IStartup>();
        startup.StartApplication();
    }
}