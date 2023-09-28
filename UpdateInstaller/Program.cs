// See https://aka.ms/new-console-template for more information
using UpdateChecker;

Console.WriteLine("Hello, World!");



var updateChecker = new GitHubUpdateChecker("Ayazis", "QuickPick");

bool result = await updateChecker.IsUpdateAvailableAsync(UpdateType.Pre_Release, new Version("0.1.1"));

Console.Read();