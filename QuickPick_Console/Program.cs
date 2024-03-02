using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace QuickPick;
public class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        IHost host = new ServicesConfig()
            .CreateHostBuilder()
            .Build();

        IStartup startup = host.Services.GetRequiredService<IStartup>();
        startup.StartApplication();
    }
}