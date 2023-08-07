using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace QuickPick.Utilities.File_Explorer;

public interface IDriveItem
{
	public string FullName { get; set; }
	public eDriveItemType Type { get; set; }
	public IDriveItem Parent { get; set; }
	public ObservableCollection<IDriveItem> Children { get; set; }
	string GetFullPath();

}
[DebuggerDisplay("{FullName}")]
public class DriveItem : IDriveItem, INotifyPropertyChanged
{
    public DriveItem()
    {
			
    }
    public DriveItem(eDriveItemType type)
	{
		Type = type;
	}
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
