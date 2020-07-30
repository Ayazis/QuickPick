
namespace QuickPick.Logic
{
    public interface ISaveLoader
    {
        Models.QuickPick QP { get; set; }

        void LoadSettingsFile();
        void LoadSettingsFile(string filePath);
        void SaveSettingsToDisk();
    }
}