using JumpList.Automatic;
using JumpList.Custom;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QuickPick
{
    public class JumpListProvider
    {
        static JumpListProvider()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            automaticDestinationDir = Path.Combine(appData, @"Microsoft\Windows\Recent\AutomaticDestinations");
            customDestinationDir = Path.Combine(appData, @"Microsoft\Windows\Recent\CustomDestinations");
        }
        static string automaticDestinationDir;
        static string customDestinationDir;


        public string GetAppIdForApplication(string path)
        {
            // Assuming you have a method to get AppId for an application
            AppIdProvider appIdProvider = new AppIdProvider();
            var appid =  appIdProvider.GetAppId(path);
            return appid;
        }
        public string[] GetRecentFiles(string applicationpath)
        {
            // get AppId for this application
            string appId = GetAppIdForApplication(applicationpath);
            var recentFiles = new List<string>();

            foreach (var filePath in Directory.GetFiles(automaticDestinationDir))
            {
                AutomaticDestination jlist = JumpList.JumpList.LoadAutoJumplist(filePath);

                if (jlist.AppId.AppId == appId)
                {
                    recentFiles.AddRange(jlist.DestListEntries.Select(s => s.Lnk.LocalPath));
                }
            }

            return recentFiles.ToArray();
        }

        public IEnumerable<AutomaticDestination> GetJumpList()
        {
            var destinationFiles = new List<AutomaticDestination>();

            foreach (var filePath in Directory.GetFiles(automaticDestinationDir))
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
