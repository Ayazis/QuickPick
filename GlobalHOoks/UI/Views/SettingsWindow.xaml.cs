using GlobalHOoks.Classes;
using GlobalHOoks.Enums;
using GlobalHOoks.Logic;
using GlobalHOoks.Models;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WindowsInput;
using FolderBrowserDialog = System.Windows.Forms.FolderBrowserDialog;


namespace GlobalHOoks
{

    public partial class SettingsWindow : Window
    {
        public QuickPick QP { get; }       

        public SettingsWindow(QuickPick QP)
        {
            this.QP = QP;
            InitializeComponent();
        }

        private void cmbClickAction_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                
                var comboBox = sender as ComboBox;

                if (comboBox.SelectedItem == null)
                    return;

                ClickAction action = (ClickAction)comboBox.SelectedItem;
                QpButton button = comboBox.DataContext as QpButton;

                QP.ButtonManager.SetClickActionOnButton(button, action);
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
            QP.SaveLoadManager.SaveSettingsToDisk();

        }
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            QP.SaveLoadManager.LoadAndApplySettings();
        }

        private void btnBrowseFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if(fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                QP.QuickPickModel.ShortCutsFolder = fbd.SelectedPath;
            }
        }
    }
}
