// See https://aka.ms/new-console-template for more information
using ArchiveFiles;
using UpdateDownloader;

Console.WriteLine("Hello, World!");

var updateChecker = new GitHubUpdateChecker("Ayazis", "QuickPick");

bool result = await updateChecker.IsUpdateAvailableAsync(UpdateType.Pre_Release, new Version("0.1.1"));
(Version version, string downloadUrl) newVersion = await updateChecker.GetLatestVersionAsync(UpdateType.Pre_Release);

Version version = newVersion.version;
string downloadUrl = newVersion.downloadUrl;

string downloadFolder = @"E:\newdownload";
FileDownloader newDownloader = new (downloadFolder);
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