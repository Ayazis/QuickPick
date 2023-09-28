using UpdateChecker;

namespace UpdateInstaller
{
    public interface IUpdateInstaller
    {
        void InstallUpdate(string filePath, UpdateType updateType);
    }

}
