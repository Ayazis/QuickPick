using ArchiveFiles;
using UpdateDownloader;
using UpdateInstaller;

public class Program
{
	public static void Main(string[] args)
	{
		// todo
		// Elevation check
		// some clever error handling.
		// logging
		InstallerArguments arguments = InstallerArguments.FromStringArray(args);
		UpdateManager updater = new UpdateManager(new ApplicationCloser(), new ArchiveExtractor());
		updater.CloseApplication(arguments.ProcessIdToKill);
		updater.ExtractUpdateFile(arguments.SourceFolder, arguments.TargetFolder);
		updater.LaunchUpdatedApplication(arguments.PathToExecutable, arguments.TargetArguments);
	}

	async Task doSomeStuffAsync()
	{
		var updateChecker = new GitHubUpdateChecker("Ayazis", "QuickPick");

		bool result = await updateChecker.IsUpdateAvailableAsync(eUpdateType.Pre_Release, new Version("0.1.1"));
		(Version version, string downloadUrl) newVersion = await updateChecker.GetLatestVersionAsync(eUpdateType.Pre_Release);

		Version version = newVersion.version;
		string downloadUrl = newVersion.downloadUrl;

		string downloadFolder = @"E:\newdownload";
		FileDownloader newDownloader = new(downloadFolder);
		newDownloader.DownloadProgressChanged += NewDownloader_DownloadProgressChanged;
		await newDownloader.DownloadFilesAsync(new Uri(downloadUrl));
		string fileName = Path.GetFileName(downloadUrl);
		string finalPath = Path.Join(downloadFolder, fileName);

		new ArchiveExtractor().ExtractFiles(finalPath, downloadFolder);


		void NewDownloader_DownloadProgressChanged(object? sender, System.Net.DownloadProgressChangedEventArgs e)
		{
			Console.WriteLine(e.ProgressPercentage);
		}

		Console.Read();

	}
}