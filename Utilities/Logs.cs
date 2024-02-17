using System.IO;

namespace Ayazis.Utilities;

public static class Logs
{
    public static ILogger Logger;

    static Logs()
    {
        Logger = new FileLogger();
        Logger.CreateDirectory();
    }
}

public class FileLogger : ILogger
{
    // get appdata/quickpick/logs
    public string LogDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "QuickPick", "Logs");
    public void CreateDirectory()
    {
        if (!Directory.Exists(LogDirectory))
            Directory.CreateDirectory(LogDirectory);
    }
    public void Log(Exception ex)
    {
        Log(ex.ToString());
    }
    public void Log(string logEntry)
    {
        if (string.IsNullOrWhiteSpace(logEntry))
            return;

        try
        {
            // Run the actual logging in a seperate task so it doesn't slow down the App's main processes.
            //Task.Run(() =>
            //{
            var dateNow = DateTime.Now.ToString("yyyyMMdd");

            string finalPath = Path.Combine(LogDirectory, $"QpLog{dateNow}.log");
            File.AppendAllText(finalPath, Environment.NewLine + logEntry);
            //});

        }
        catch (Exception)
        {
            // do nothing.
        }
    }

}

public interface ILogger
{   
    void Log(string entry);
    void Log(Exception ex);

    void CreateDirectory();

}
