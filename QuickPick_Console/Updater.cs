using System.Diagnostics;
using System.Net;
using System.Reflection;
using UpdateDownloader;

namespace QuickPick;
public class Updater
{

	GitHubUpdateChecker _updateChecker = new GitHubUpdateChecker("Ayazis", "QuickPick");
	public async Task<bool> IsUpdateAvailableAsync()
	{
		Assembly assembly = Assembly.GetExecutingAssembly();
		Version currentVersion = assembly.GetName().Version;
		bool updateAvailable = await _updateChecker.IsUpdateAvailableAsync(eUpdateType.Pre_Release, currentVersion);
		return updateAvailable;
	}
	public async Task InstallUpdateAsync()
	{
		(Version version, string downloadUrl) update = await _updateChecker.GetLatestVersionAsync(eUpdateType.Pre_Release);
		Version newVersion = update.version;
		string downloadUrl = update.downloadUrl;

		string pathToCurrentExecutable = Process.GetCurrentProcess().MainModule.FileName;
		string currentFolder = Path.GetDirectoryName(pathToCurrentExecutable);
		string downloadFolder = Path.Join(currentFolder, "Update");
		FileDownloader newDownloader = new(downloadFolder);
		newDownloader.DownloadProgressChanged += NewDownloader_DownloadProgressChanged;
		string fileName = Path.GetFileName(downloadUrl);
		string updateZipPath = Path.Join(downloadFolder, fileName);
		await newDownloader.DownloadFilesAsync(new Uri(downloadUrl));// todo return downloaded filepath

		string targetfolder = currentFolder;
		int currentProcessId = Process.GetCurrentProcess().Id;
		InstallerArguments arguments = new InstallerArguments(updateZipPath, targetfolder, currentProcessId, pathToCurrentExecutable, null);
	}

	static void NewDownloader_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
	{
	//	throw new NotImplementedException();
	}



}
