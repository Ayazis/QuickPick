using Microsoft.Extensions.DependencyInjection;
namespace QuickPick;
public class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        var deviceManager = new BluetoothManager();
        var devices = deviceManager.GetPairedDevices();
        deviceManager.ConnectToDevice("WF-1000XM4");

        IStartup startup = ServicesConfig.AppHost.Services.GetRequiredService<IStartup>();
        startup.StartApplication();
    }
}