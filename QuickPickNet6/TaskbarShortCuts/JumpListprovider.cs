using JumpList.Automatic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
namespace QuickPick
{
    public class JumpListProvider
    {
        string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string recentFilesDir => Path.Combine(appData, @"Microsoft\Windows\Recent\AutomaticDestinations");
        public IEnumerable<AutomaticDestination> GetJumpList()
        {
          var destinationFiles = new List<AutomaticDestination>();

            foreach (var filePath in Directory.GetFiles(recentFilesDir))
            {
                AutomaticDestination jlist = JumpList.JumpList.LoadAutoJumplist(filePath);

                // todo: Find out which files are recently opened by which program.
                destinationFiles.Add(jlist);
            }
            return destinationFiles;
        }
    }
}
