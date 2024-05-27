using Microsoft.Extensions.DependencyInjection;
namespace QuickPick;
public class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        IStartup startup = ServicesConfig.AppHost.Services.GetRequiredService<IStartup>();
        startup.StartApplication();
    }
}