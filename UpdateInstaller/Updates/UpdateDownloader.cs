using InstallerExecutableLauncher;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using Updates;
using UpdateInstaller.Updates;
using UpdateInstaller;

namespace QuickPick;
public class UpdateDownloader
{
    IUpdateChecker _updateChecker;
    eUpdateType _updateType;
    public UpdateDownloader(eUpdateType updateType, IUpdateChecker updateChecker)
    {
        _updateType = updateType;
        _updateChecker = updateChecker;
    }
    public async Task<bool> IsUpdateAvailableAsync()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        Version currentVersion = assembly.GetName().Version;
    #if DEBUG
        currentVersion = new Version("0.0.0");
    #endif

        bool updateAvailable = await _updateChecker.IsUpdateAvailableAsync(_updateType, currentVersion);
        return updateAvailable;
    }

    public async Task<Version> GetLatestVersionAsync()
    {
        (Version version, string downloadUrl) update = await _updateChecker.GetLatestVersionAsync(_updateType);
        return update.version;
    }

    /// <summary>
    /// Downloads file and returns final path to file.
    /// </summary>
    /// <returns></returns>
    public async Task<string> DownloadUpdateAsync()
    {
        (Version version, string downloadUrl) update = await _updateChecker.GetLatestVersionAsync(_updateType);
        Version newVersion = update.version;
        string downloadUrl = update.downloadUrl;
  
        string appDataPathFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string downloadFolder = Path.Join(appDataPathFolder, "QuickPickUpdate");
        FileDownloader newDownloader = new(downloadFolder);
        newDownloader.DownloadProgressChanged += NewDownloader_DownloadProgressChanged;
        string fileName = Path.GetFileName(downloadUrl);
        string updateZipPath = Path.Join(downloadFolder, fileName);
        await newDownloader.DownloadFilesAsync(new Uri(downloadUrl));// todo return downloaded filepath
        this.DownloadCompleted?.Invoke(this, null);

        return updateZipPath;
    }

    public void NewDownloader_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
    {
        DownloadProgressChanged?.Invoke(this, e);
    }

    public event EventHandler<DownloadProgressChangedEventArgs> DownloadProgressChanged;
    public event EventHandler DownloadCompleted;
}
