﻿using QuickPick.Models;
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

            if (!Directory.Exists(pathToFiles))
                return;

            var files = Directory.GetFiles(pathToFiles, "*", SearchOption.AllDirectories);

            var newshortCuts = new List<ShortCut>();

            foreach (var file in files)
            {
                var targetPath = GetTargetPath(file);
                var icon = GetIcon(targetPath) ?? GetIcon(file);           

                if (!string.IsNullOrWhiteSpace(targetPath) && icon != null)
                {
                    newshortCuts.Add(new ShortCut { Icon = icon, TargetPath = targetPath });
                }
                else
                {

                }

            }

            if (newshortCuts.Count>0)
            {
                qpm.ShortCuts = new System.Collections.ObjectModel.ObservableCollection<ShortCut>(newshortCuts);
                
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
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                    return null;

                if (!System.IO.File.Exists(path))
                    return null;

                return Icon.ExtractAssociatedIcon(path);
            }
            catch (Exception ex)
            {

                return null;
            }   
        }
    }
}
