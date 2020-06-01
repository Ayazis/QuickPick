using GlobalHOoks.Classes;
using GlobalHOoks.Enums;
using GlobalHOoks.Logic;
using GlobalHOoks.Models;
using Shell32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

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
            var files = Directory.GetFiles(pathToFiles);

            foreach (var file in files)
            {
                var shortcut = new ShortCut();
                string iconLocation = "";

                try
                {
                    string pathOnly = Path.GetDirectoryName(file);
                    string filenameOnly = Path.GetFileName(file);

                    Shell shell = new Shell();
                    Folder folder = shell.NameSpace(pathOnly);
                    FolderItem folderItem = folder.ParseName(filenameOnly);
                    if (folderItem != null)
                    {
                        ShellLinkObject link = folderItem.GetLink as ShellLinkObject;
                        if (link == null)
                            continue;

                        shortcut.TargetPath = link.Path;


                        link.GetIconLocation(out iconLocation);
                        if (string.IsNullOrEmpty(iconLocation))
                        {
                            iconLocation = link.Path;
                        }


                        iconLocation = iconLocation.Replace("%SystemRoot%", @"c:\windows");

                        shortcut.IconLocation = iconLocation;
                        shortcut.Icon = System.Drawing.Icon.ExtractAssociatedIcon(iconLocation);

                    }
                }
                catch (Exception ex)
                {

                    Logger.Log(ex.ToString());
                    Logger.Log(iconLocation);
                }
                finally
                {
                    if (!string.IsNullOrWhiteSpace(shortcut.TargetPath))
                    {
                        ShortCuts.Add(shortcut);
                    }

                }
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
