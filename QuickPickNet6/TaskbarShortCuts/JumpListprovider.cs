using System.IO;
using System;
using System.Diagnostics;
using OpenMcdf;

namespace QuickPick.UI.TaskbarShortCuts;
public class JumpListprovider
{
	public void GetShellLinks()
	{
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string targetPath = Path.Combine(appDataPath, @"Microsoft\Windows\Recent\AutomaticDestinations");

        foreach (var filePath in Directory.GetFiles(targetPath))
        {
            CompoundFile cf = new CompoundFile(filePath);
            
          ProcessStorage(cf.RootStorage, string.Empty);
            cf.Close();
        }
    }

    public void ProcessStorage(CFStorage stg, string path)
    {

        stg.VisitEntries(entry =>
        {
            string entryPath = path + "\\" + entry.Name;
            if (entry is CFStream)
            {
                Debug.WriteLine("Stream: " + entryPath);
            }
            else if (entry is CFStorage)
            {
                Debug.WriteLine("Storage: " + entryPath);
                ProcessStorage((CFStorage)entry, entryPath);
            }
        }, false);
    }


}
