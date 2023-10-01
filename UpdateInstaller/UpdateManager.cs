using ArchiveFiles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateInstaller;
internal interface IUpdateManager
{
	void ExtractUpdateFile(string sourcePath, string targetFolder);
	void CloseApplication(int processId);
	void LaunchUpdatedApplication(string targetPath, string[] args);

}

public class UpdateManager : IUpdateManager
{
	IApplicationCloser _appCloser;
	IArchiveExtractor _archiveExtractor;

	public UpdateManager(IApplicationCloser appCloser, IArchiveExtractor archiveExtractor)
	{
		_appCloser = appCloser;
		_archiveExtractor = archiveExtractor;
	}

	public void CloseApplication(int processId)
	{
		_appCloser.CloseApplication(processId);
	}

	public void ExtractUpdateFile(string sourcePath, string targetFolder)
	{
		_archiveExtractor.ExtractFiles(sourcePath, targetFolder);
	}

	public void LaunchUpdatedApplication(string targetPath, string[]? args = null)
	{
		if (args != null)
			Process.Start(targetPath, args);

		else
			Process.Start(targetPath);
	}
}
