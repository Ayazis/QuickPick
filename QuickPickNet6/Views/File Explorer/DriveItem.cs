using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace QuickPick.UI.Views.File_Explorer;

public interface IDriveItem
{
    public string FullName { get; set; }
    public eDriveItemType Type { get; set; }
    public IDriveItem Parent { get; set; }
    public ObservableCollection<IDriveItem> Children { get; set; }
    string GetFullPath();

}
public class DriveItem : IDriveItem, INotifyPropertyChanged
{
    public string FullName { get; set; }
    public eDriveItemType Type { get; set; }
    public IDriveItem Parent { get; set; }
    public ObservableCollection<IDriveItem> Children { get; set; } = new();

    public string GetFullPath()
    {
        if (Parent == null)
            return FullName;

        return Path.Combine(Parent.GetFullPath(), FullName);
    }
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }


}
