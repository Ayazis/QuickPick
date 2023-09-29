using FileDownloader;

namespace UpdateInstaller
{
    public interface IUpdateInstaller
    {
        void InstallUpdate(string filePath, UpdateType updateType);
    }

}
