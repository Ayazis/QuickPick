// See https://aka.ms/new-console-template for more information
using FileDownloader;

Console.WriteLine("Hello, World!");

var updateChecker = new GitHubUpdateChecker("Ayazis", "QuickPick");

bool result = await updateChecker.IsUpdateAvailableAsync(UpdateType.Pre_Release, new Version("0.1.1"));
var newVersion = await updateChecker.GetLatestVersionAsync(UpdateType.Pre_Release);

var version = newVersion.version;
var downloadUrl = newVersion.downloadUrl;

Console.Read();