using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Taskbar;

public class PinnedAppManager
{
    private readonly string _categoryName;

    public PinnedAppManager(string categoryName)
    {
        _categoryName = categoryName;
    }

    public JumpList GetJumbList()
    {
        // Get the JumpList for the application
        JumpList jumpList = JumpList.CreateJumpList();        
        return jumpList;
    }
}
