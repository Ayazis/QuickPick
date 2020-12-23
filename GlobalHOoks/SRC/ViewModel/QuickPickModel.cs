using QuickPick.Classes;
using QuickPick.Enums;
using QuickPick.Logic;
using QuickPick.Models;
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
using System.Linq;

namespace QuickPick
{
    public class QuickPickModel : INotifyPropertyChanged
    {


        public string SettingsPath
        {
            get
            {
                return Directory.GetCurrentDirectory() + "/QuickPickSettings.Json";
            }
        }

        private string _Title = "QuickPick Settings                v.1.2.1alpha";
        public string Title
        {
            get { return _Title; }
        }

        private string _ShortCutsFolder = @"c:\shortcuts";
        public string ShortCutsFolder
        {
            get { return _ShortCutsFolder; }
            set
            {
                _ShortCutsFolder = value;
                NotifyPropertyChanged(nameof(ShortCutsFolder));
            }
        }


        private bool _SettingsAreSaved = false;

        public bool SettingsAreSaved
        {
            get { return _SettingsAreSaved; }
            set
            {
                _SettingsAreSaved = value;
                NotifyPropertyChanged(nameof(SettingsAreSaved));
            }
        }

        public HotKey Hotkey { get; set; } = HotKey.KeyCombination;

        public bool InstantShortCuts { get; set; } = false;

        #region Properties        
        private ObservableCollection<Keys> _HotKeys = new ObservableCollection<Keys> {
        Keys.LControlKey,
        Keys.LMenu,
        Keys.LShiftKey,
        Keys.Q,
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
