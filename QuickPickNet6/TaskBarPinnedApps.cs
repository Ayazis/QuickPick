using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

public class TaskbarPinnedApps
{
    private const string TASKBAR_FOLDERPATH = @"Microsoft\Internet Explorer\Quick Launch\User Pinned\TaskBar";

    public class PinnedAppInfo
    {
        public string Name { get; set; }
        public string TargetPath { get; set; }
        public Icon AppIcon { get; set; }
    }

    public static List<PinnedAppInfo> GetPinnedTaskbarApps()
    {
        List<PinnedAppInfo> pinnedApps = new List<PinnedAppInfo>();

        string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string taskbarFolder = Path.Combine(appData, TASKBAR_FOLDERPATH);

        if (!Directory.Exists(taskbarFolder))
            return null;

        var pinnedAppPaths = Directory.GetFiles(taskbarFolder, "*.lnk", new EnumerationOptions { RecurseSubdirectories = true });

        foreach (string pinnedAppPath in pinnedAppPaths)
        {
            var shortcut = new Shell32.Shell().NameSpace(Path.GetDirectoryName(pinnedAppPath)).ParseName(Path.GetFileName(pinnedAppPath));
            string targetPath = shortcut.GetLink.Target.Path;

            if (!string.IsNullOrEmpty(targetPath))
            {
                PinnedAppInfo appInfo = new PinnedAppInfo()
                {
                    Name = Path.GetFileNameWithoutExtension(targetPath),
                    TargetPath = targetPath,
                    AppIcon = ExtractIcon(targetPath)
                };

                pinnedApps.Add(appInfo);
            }
        }

        return pinnedApps;
    }

    private static Icon ExtractIcon(string targetPath)
    {
        IntPtr[] largeIcons = new IntPtr[1];
        IntPtr[] smallIcons = new IntPtr[1];

        ExtractIconEx(targetPath, 0, largeIcons, smallIcons, 1);

        if (largeIcons[0] != IntPtr.Zero)
        {
            Icon icon = Icon.FromHandle(largeIcons[0]);
            return icon;
        }
        else if (smallIcons[0] != IntPtr.Zero)
        {
            Icon icon = Icon.FromHandle(smallIcons[0]);
            return icon;
        }

        return null;
    }
    [DllImport("shell32.dll", CharSet = CharSet.Auto)]
    private static extern uint ExtractIconEx(string szFileName, int nIconIndex, IntPtr[] phiconLarge, IntPtr[] phiconSmall, uint nIcons);
}
