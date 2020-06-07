using GlobalHOoks.Classes;
using GlobalHOoks.Enums;
using GlobalHOoks.Logic;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WindowsInput;


namespace GlobalHOoks
{

    public partial class SettingsWindow : Window
    {
        private QuickPickModel _qpm;
        private WindowManager _WindowManager;
        private ButtonManager _buttonManager;
        private SaveLoadManager _saveLoadManager;

        public SettingsWindow(QuickPickModel model, WindowManager manager, ButtonManager buttonManager)
        {
            _qpm = model;
            _WindowManager = manager;
            _buttonManager = buttonManager;
            InitializeComponent();
        }

        private void cmbClickAction_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                var comboBox = sender as ComboBox;
                ClickAction action = (ClickAction)comboBox.SelectedItem;
                QpButton button = comboBox.DataContext as QpButton;

                _buttonManager.SetClickActionOnButton(button, action);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }

        private void btnChooseFile_Click(object sender, RoutedEventArgs e)
        {
            // show fileDialog, save selectedFile to button's associated file

            var button = sender as Button;
            QpButton qpButton = button.DataContext as QpButton;

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Select File";
            fileDialog.InitialDirectory = @"C:\";
            if (fileDialog.ShowDialog() == true )
            {
                qpButton.AssociatedFilePath = fileDialog.FileName;
            }

        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            // save all button settings to disk.
            if (_saveLoadManager == null)
                _saveLoadManager = new SaveLoadManager(_qpm);

            _saveLoadManager.SaveSettingsToDisk();

        }
    }
}
