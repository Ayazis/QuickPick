namespace FileDownloader;

public enum UpdateType
{
    Stable,
    Pre_Release
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
