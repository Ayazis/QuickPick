namespace UpdateChecker;

public enum UpdateType
{
    Stable,
    Pre_Release
}

public interface IFileDownloader
{
    Task DownloadFilesAsync(Uri source, string destination);
    event EventHandler<DownloadProgressEventArgs> DownloadProgressChanged;
}


public interface IUpdateManager
{
    Task<bool> CheckAndUpdateAsync(UpdateType updateType);
}
public interface IApplicationCloser
{
    void CloseApplication();
}

public interface IInstallerLauncher
{
    void LaunchInstaller(string installerPath, string arguments);
}
public class DownloadProgressEventArgs : EventArgs
{
    public int ProgressPercentage { get; set; }
    public long BytesReceived { get; set; }
    public long TotalBytesToReceive { get; set; }
}