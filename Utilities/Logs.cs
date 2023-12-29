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
    public string LogPath { get;} = @"C:\temp\QuickPicLogs\";
    public void CreateDirectory()
    {
        if (!Directory.Exists(LogPath))
            Directory.CreateDirectory(LogPath);
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

                File.AppendAllText($@"{LogPath}QpLog{dateNow}.txt", Environment.NewLine+logEntry);
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
    string LogPath { get; }

    void Log(string entry);
    void Log(Exception ex);

    void CreateDirectory();

}
