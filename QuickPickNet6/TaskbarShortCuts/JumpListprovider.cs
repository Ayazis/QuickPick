using System;
using System.Windows;
using ;

namespace QuickPick.UI.TaskbarShortCuts;
internal class JumpListprovider
{
	public void tryGet(Application application)
	{
		System.Windows.Shell.JumpList appJumpList = System.Windows.Shell.JumpList.GetJumpList(application) ;
		foreach (System.Windows.Shell.JumpItem item in appJumpList.JumpItems)
		{
			
		}
	}	
	public void tryGet(Application application)
	{
		Microsoft.WindowsAPICodePack.Taskbar.JumpList appJumpList = Microsoft.WindowsAPICodePack.Taskbar.JumpList.CreateJumpListForIndividualWindow();


		// try https://stackoverflow.com/questions/6679926/how-to-programmatically-open-jumplist-window-for-specific-exe-file
	}
}
