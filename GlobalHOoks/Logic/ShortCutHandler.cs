using GlobalHOoks.Models;
using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalHOoks.Logic
{
    static class ShortCutHandler
    {        
        public static void GetShortCuts(QuickPickModel qpm)
        {            

            string pathToFiles = qpm.ShortCutsFolder;     
            var files = Directory.GetFiles(pathToFiles, "*", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                var icon = GetIcon(file);
                var targetpath = GetTargetPath(file);

                if (!string.IsNullOrWhiteSpace(targetpath) && icon != null)
                {
                    qpm.ShortCuts.Add(new ShortCut { Icon = icon, TargetPath = targetpath });
                }

            }

        }

        private static string GetTargetPath(string path)
        {
            try
            {
                if (System.IO.File.Exists(path))
                {
                    // WshShellClass shell = new WshShellClass();
                    WshShell shell = new WshShell(); //Create a new WshShell Interface
                    IWshShortcut link = (IWshShortcut)shell.CreateShortcut(path); //Link the interface to our shortcut

                    return link.TargetPath;
                }

                return null;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return null;
            }

        }

        private static Icon GetIcon(string path)
        {
            Icon icon = null;
            try
            {
                if (System.IO.File.Exists(path))
                {
                    //Create a new WshShell Interface
                    WshShell shell = new WshShell();
                    //Link the interface to our shortcut
                    IWshShortcut link = (IWshShortcut)shell.CreateShortcut(path);
                    if (string.Empty != link.TargetPath)
                        icon = Icon.ExtractAssociatedIcon(link.TargetPath);
                }

                return icon;
            }
            catch (Exception)
            {
                return icon;
            }
        }
    }
}
