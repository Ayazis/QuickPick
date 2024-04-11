using CommunityToolkit.Mvvm.ComponentModel;
using QuickPick.PinnedApps;
using System.Collections.ObjectModel;

namespace QuickPick;

public partial class QuickPickMainWindowModel : ObservableObject
{
    public ObservableCollection<AppLink> QuickLinks { get; set; } = new() 
    {
        new AppLink(),
        new AppLink(),
        new AppLink(),
        new AppLink(),
        new AppLink(),
        new AppLink(),        
    };
    public ObservableCollection<AppLink> PinnedApps { get; set; } = new();
    public ObservableCollection<string> ButtonLabels { get; set; } = new();
    [ObservableProperty]
    private string _ShortCutsFolder = @"c:\shortcuts";
}
