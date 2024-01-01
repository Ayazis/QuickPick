using QuickPick.Utilities.File_Explorer;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace QuickPick.UI.Views.File_Explorer;

public class File_Explorer_DataContext
{
    public void SetDriveItems(IEnumerable<IDriveItem> driveItems)
    {
        DriveItems = new ObservableCollection<IDriveItem>(driveItems);
    }

    public ObservableCollection<IDriveItem> DriveItems { get; set; } = new ObservableCollection<IDriveItem>()
    {
        new DriveItem(eDriveItemType.Drive){ FullName =  "C:"}
    };
}
