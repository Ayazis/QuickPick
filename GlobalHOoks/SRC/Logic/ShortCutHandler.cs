using QuickPick.Models;
using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickPick.Logic
{
    static class ShortCutHandler
    {        
        public static void GetShortCuts(QuickPickModel qpm)
        {            

            string pathToFiles = qpm.ShortCutsFolder;     
            var files = Directory.GetFiles(pathToFiles, "*", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                var targetPath = GetTargetPath(file);
                var icon = GetIcon(targetPath) ?? GetIcon(file);           

                if (!string.IsNullOrWhiteSpace(targetPath) && icon != null)
                {
                    qpm.ShortCuts.Add(new ShortCut { Icon = icon, TargetPath = targetPath });
                }
                else
                {

                }

            }

        }

        private static string GetTargetPath(string path)
        {
            try
            {
                if (System.IO.File.Exists(path))
                {
                    string targetPath = null;

                    WshShell shell = new WshShell();

                    dynamic newShortCut = shell.CreateShortcut(path);

                    // Both IwshShortCut and IwshURlShortCut have the TargetPath Property.
                    targetPath = newShortCut.TargetPath;                   
                    
                    return targetPath;                   
                }

                return null;
            }
            catch (Exception ex)
            {
                Logs.Logger.Log(ex);
                return null;
            }

        }

        public static Icon GetIcon(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return null;

            return Icon.ExtractAssociatedIcon(path);            
        }
    }
}
