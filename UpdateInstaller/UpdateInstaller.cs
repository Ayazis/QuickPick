using UpdateDownloader;

namespace UpdateInstaller
{
	public interface IUpdateInstaller
    {
        void InstallUpdate(string sourceFolder, string targetFolder, string pathToExecutable);
    }   
}
