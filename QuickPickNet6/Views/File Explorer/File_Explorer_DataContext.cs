using System.Collections.ObjectModel;
namespace QuickPick.UI.Views.File_Explorer;

public class File_Explorer_DataContext
{
    public ObservableCollection<IDriveItem> DriveItems { get; set; } = new();
}
