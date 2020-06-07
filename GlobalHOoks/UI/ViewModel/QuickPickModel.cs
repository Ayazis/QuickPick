using GlobalHOoks.Classes;
using GlobalHOoks.Enums;
using GlobalHOoks.Logic;
using GlobalHOoks.Models;
using IWshRuntimeLibrary;
using Shell32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Point = System.Windows.Point;

namespace GlobalHOoks
{
    public class QuickPickModel : INotifyPropertyChanged
    {
        #region Properties


        private ObservableCollection<Keys> _HotKeys = new ObservableCollection<Keys> {
        Keys.LControlKey,
        Keys.LShiftKey,
        Keys.LMenu,
        Keys.Q
        };
        public ObservableCollection<Keys> HotKeys
        {
            get { return _HotKeys; }
            set
            {
                _HotKeys = value;
                NotifyPropertyChanged(nameof(HotKeys));
            }
        }

        private ObservableCollection<ShortCut> _ShortCuts = new ObservableCollection<ShortCut>();
        public ObservableCollection<ShortCut> ShortCuts
        {
            get { return _ShortCuts; }
            set
            {
                _ShortCuts = value;
                NotifyPropertyChanged(nameof(ShortCuts));
            }
        }

        private ObservableCollection<QpButton> _ShortCutButtons = new ObservableCollection<QpButton>();
        public ObservableCollection<QpButton> ShortCutButtons
        {
            get { return _ShortCutButtons; }
            set
            {
                _ShortCutButtons = value;
                NotifyPropertyChanged(nameof(ShortCutButtons));
            }
        }



        private ObservableCollection<QpButton> _MainButtons = new ObservableCollection<QpButton>();
        public ObservableCollection<QpButton> MainButtons
        {
            get { return _MainButtons; }
            set
            {
                _MainButtons = value;
                NotifyPropertyChanged(nameof(MainButtons));
            }
        }

        public int NrOfButtons { get; set; } = 8;

        private int _CircleRadius = 50;
        public int CircleRadius
        {
            get { return _CircleRadius; }
            set
            {
                _CircleRadius = value;
                NotifyPropertyChanged(nameof(CircleRadius));
            }
        }
        public Point Center
        {
            get { return new Point(WidthHeight / 2, WidthHeight / 2); }
        }
        public int WidthHeight
        {
            get { return 200; }
        }
        #endregion

        public QuickPickModel()
        {
            GetShortCuts();
        }
        private void GetShortCuts()
        {
            string pathToFiles = @"C:\Shortcuts\"; 
            //string pathToFiles = @"C:\Shortcuts\Office Related";
            var files = Directory.GetFiles(pathToFiles, "*", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                var icon = GetIcon(file);
                var targetpath = GetTargetPath(file);

                if (!string.IsNullOrWhiteSpace(targetpath) && icon != null)
                {
                    ShortCuts.Add(new ShortCut { Icon = icon, TargetPath = targetpath });
                }
              
            }

        }

        private string GetTargetPath(string path)
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

        private Icon GetIcon(string path)
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

        #region Notify Property Changed And other Events
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion


    }
}
