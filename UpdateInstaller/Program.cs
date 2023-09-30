// See https://aka.ms/new-console-template for more information
using UpdateDownloader;

Console.WriteLine("Hello, World!");

var updateChecker = new GitHubUpdateChecker("Ayazis", "QuickPick");

bool result = await updateChecker.IsUpdateAvailableAsync(UpdateType.Pre_Release, new Version("0.1.1"));
(Version version, string downloadUrl) newVersion = await updateChecker.GetLatestVersionAsync(UpdateType.Pre_Release);

Version version = newVersion.version;
string downloadUrl = newVersion.downloadUrl;

FileDownloader newDownloader = new (@"E:\newdownload");
newDownloader.DownloadProgressChanged += NewDownloader_DownloadProgressChanged;
await newDownloader.DownloadFilesAsync(new Uri(downloadUrl));



void NewDownloader_DownloadProgressChanged(object? sender, System.Net.DownloadProgressChangedEventArgs e)
{
	Console.WriteLine(e.ProgressPercentage);
}

Console.Read();