using QuickPick;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Updates;

namespace UpdateInstaller.Updates;

public class UpdateDownloadManager
{
    IUpdateDownloader _updateDownloader;

    public UpdateDownloadManager(IUpdateDownloader updateDownloader)
    {
        _updateDownloader = updateDownloader;
    }

    public async Task<bool> CheckIfUpdateIsAvailableAsync()
    {
        return await _updateDownloader.IsUpdateAvailableAsync();
    }
    public InstallerParams DownloadUpdateAndGetInstallerArguments()
    {
        // .Result blocks the UI Thread so we cannot update it,
        // can be fixed by calling an event with the url when it is done
        string downloadedFile = _updateDownloader.DownloadUpdateAsync().Result;
        string extractionFolder = CreateTargetDirectory(downloadedFile);
        new ArchiveExtractor().ExtractFiles(downloadedFile, extractionFolder);

        string pathToCurrentExecutable = Process.GetCurrentProcess().MainModule.FileName;
        string currentExecutableName = Path.GetFileName(pathToCurrentExecutable);
        string currentDirectory = Path.GetDirectoryName(pathToCurrentExecutable);
        string newInstallerPath = Path.Join(extractionFolder, currentExecutableName);

        int processId = Process.GetCurrentProcess().Id;

        var installerArguments = new InstallerArguments(
            sourceFolder: extractionFolder,
            targetFolder: currentDirectory,
            processIdToKill: processId,
            pathToExecutable: pathToCurrentExecutable
            , arguments: Array.Empty<string>());


        return new InstallerParams(newInstallerPath, installerArguments); ;

    }

    /// <summary>
    /// Creates a folder with the name of the zipfile, in the location of the zipfile. 
    /// Basically does an 'Extract here'.
    /// </summary>
    /// <param name="downloadedFile"></param>
    /// <returns></returns>
    private static string CreateTargetDirectory(string downloadedFile)
    {
        string targetDirectory = Path.GetDirectoryName(downloadedFile);
        string targetFolderName = Path.GetFileNameWithoutExtension(downloadedFile) + "\\";
        string extractionFolder = Path.Join(targetDirectory, targetFolderName);

        if (!Directory.Exists(extractionFolder))
            Directory.CreateDirectory(extractionFolder);

        return extractionFolder;
    }

}
