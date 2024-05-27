using InstallerExecutableLauncher;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using UpdateDownloader;

namespace QuickPick;
public class Updater
{
	public Updater(eUpdateType updateType)
	{
		_updateType = updateType;
	}
	GitHubUpdateChecker _updateChecker = new GitHubUpdateChecker("Ayazis", "QuickPick");
	eUpdateType _updateType;

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

	public async Task DownloadAndInstallUpdateAsync()
	{
		(Version version, string downloadUrl) update = await _updateChecker.GetLatestVersionAsync(_updateType);
		Version newVersion = update.version;
		string downloadUrl = update.downloadUrl;

		string pathToCurrentExecutable = Process.GetCurrentProcess().MainModule.FileName;
		string currentFolder = Path.GetDirectoryName(pathToCurrentExecutable);
		string appDataPathFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		string downloadFolder = Path.Join(appDataPathFolder, "QuickPickUpdate");
		FileDownloader newDownloader = new(downloadFolder);
		newDownloader.DownloadProgressChanged += NewDownloader_DownloadProgressChanged;
		string fileName = Path.GetFileName(downloadUrl);
		string updateZipPath = Path.Join(downloadFolder, fileName);
		await newDownloader.DownloadFilesAsync(new Uri(downloadUrl));// todo return downloaded filepath
		this.DownloadCompleted?.Invoke(this, null);

		string targetfolder = currentFolder;
		int currentProcessId = Process.GetCurrentProcess().Id;
		InstallerArguments arguments = new InstallerArguments(updateZipPath, targetfolder, currentProcessId, pathToCurrentExecutable, null);

		string pathToInstaller = Path.Join(downloadFolder, "updateInstaller.exe");
		new InstallerLauncher().LaunchInstaller(pathToInstaller, arguments.ToStringArray());

	}

	public void NewDownloader_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
	{
		DownloadProgressChanged?.Invoke(this, e);
	}

	public event EventHandler<DownloadProgressChangedEventArgs> DownloadProgressChanged;
	public event EventHandler DownloadCompleted;
}
