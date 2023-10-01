using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateInstaller;
internal interface IUpdateManager
{
	void ExtractUpdateFile(string sourcePath, string TargetPath);
	void CloseApplication(int processId);
	void InstallUpdate(string sourceFolder, string TargetFolder);
	void LaunchUpdatedApplication(string targetPath);

}
