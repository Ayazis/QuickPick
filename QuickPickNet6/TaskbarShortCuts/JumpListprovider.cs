using JumpList.Automatic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
namespace QuickPick
{
    public class JumpListProvider
    {
        static JumpListProvider()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            recentFilesDir = Path.Combine(appData, @"Microsoft\Windows\Recent\AutomaticDestinations");
        }
        static string recentFilesDir;
        public IEnumerable<AutomaticDestination> GetJumpList()
        {
            var destinationFiles = new List<AutomaticDestination>();

            foreach (var filePath in Directory.GetFiles(recentFilesDir))
            {
                AutomaticDestination jlist = JumpList.JumpList.LoadAutoJumplist(filePath);

                // todo: Find out which files are recently opened by which program.
                destinationFiles.Add(jlist);
                var filesOpened = jlist.DestListEntries.Select(s => s.Lnk);
            }



            return destinationFiles;
        }
    }
}
