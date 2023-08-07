using System.Collections.ObjectModel;
using System.IO;
namespace QuickPick.Utilities.File_Explorer;

public interface IFileManager
{
	IEnumerable<IDriveItem> GetChildNodes(IDriveItem parentNode);
	IEnumerable<IDriveItem> GetLocalDrives();
}

public class FileManager : IFileManager
{
	IDriveItem _root = new DriveItem(eDriveItemType.Root);
	// Gets all the local drives on the system.
	public IEnumerable<IDriveItem> GetLocalDrives()
	{
		try
		{
			DriveInfo[] driveInfos = DriveInfo.GetDrives();
			var drives = driveInfos.Select(drive => new DriveItem()
			{
				Type = eDriveItemType.Drive,
				FullName = drive.Name,
				Parent = null
			});
			_root.Children = new ObservableCollection<IDriveItem>(drives);
			return drives;
		}
		catch (Exception)
		{
			return new List<IDriveItem>();
		}
	}

	public IEnumerable<IDriveItem> GetChildNodes(IDriveItem parentNode)
	{
		try
		{
			DirectoryInfo directory = new DirectoryInfo(parentNode.GetFullPath());
			FileSystemInfo[] fileSystemInfos = directory.GetFileSystemInfos(); // This method returns both files and directories.

			IEnumerable<DriveItem> childNodes = fileSystemInfos.Select(childNode => new DriveItem()
			{
				Type = childNode is FileInfo ? eDriveItemType.File : eDriveItemType.Folder,
				FullName = childNode is FileInfo ? childNode.Name + childNode.Extension : childNode.Name,
				Parent = parentNode,
			});
			parentNode.Children = new ObservableCollection<IDriveItem>(childNodes);
			return childNodes;
		}
		catch (Exception)
		{
			return new List<IDriveItem>();
		}
	}
}