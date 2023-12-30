using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using QuickPick.PinnedApps;

namespace QuickPick;

public partial class QuickPickMainWindowModel : ObservableObject
{
	public ObservableCollection<AppLink> PinnedApps { get; set; } = new();
	public ObservableCollection<string> ButtonLabels { get; set; } = new();
    [ObservableProperty]
    private string _ShortCutsFolder = @"c:\shortcuts";    	
}
