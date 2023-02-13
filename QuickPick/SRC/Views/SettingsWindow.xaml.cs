using QuickPick.Classes;
using QuickPick.Enums;
using QuickPick.Logic;
using QuickPick.Models;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WindowsInput;
using FolderBrowserDialog = System.Windows.Forms.FolderBrowserDialog;
using System.Windows.Forms.Integration;
using Newtonsoft.Json;
using Forms = System.Windows.Forms;

namespace QuickPick
{
    public partial class SettingsWindow : Window
    {
               
        public Models.QuickPick QP { get; }

        private QuickPickSettings _settings;
        public SettingsWindow(Models.QuickPick QP)
        {
            this.QP = QP;
            this.QP.QuickPickModel.SettingsAreSaved = false;
            InitializeComponent();
            ElementHost.EnableModelessKeyboardInterop(this);
            _settings = new QuickPickSettings(QP.QuickPickModel);
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
                Logs.Logger.Log(ex);
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
            QP.SaveLoader.ExportSettings();
            QP.SaveLoader.LoadSettingsFile();


        }
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            QP.SaveLoader.LoadSettingsFile();
        }

        private void btnBrowseFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if(fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                QP.QuickPickModel.ShortCutsFolder = fbd.SelectedPath;

                // Get Shortcuts from saved folderLocation.
                // Get Shortcuts from saved folderLocation.
                QP.QuickPickModel.ShortCuts.Clear();
                ShortCutHandler.GetShortCuts(QP.QuickPickModel);
                QP.ButtonManager.AddShortCuts();

                QP.ButtonManager.ClearCanvas();
                QP.ButtonManager.AddCentralButton();
                QP.ButtonManager.PlaceButtonsOnCanvas();
                QP.ButtonManager.AddShortCuts();
            }
        }

        private void btnAddButton_Click(object sender, RoutedEventArgs e)
        {
            var b = new QpButton();
            QP.ButtonManager.ConfigureButton(b);
            QP.QuickPickModel.MainButtons.Add(b);
        }

        private void btnRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var count = QP.QuickPickModel.MainButtons.Count;
            QP.QuickPickModel.MainButtons.RemoveAt(count - 1);
        }     

        private void rdbOnCenter_Checked(object sender, RoutedEventArgs e)
        {
            QP.QuickPickModel.InstantShortCuts = false;

        }

        private void rdbInstant_Checked(object sender, RoutedEventArgs e)
        {
            QP.QuickPickModel.InstantShortCuts = true;
        }

        private void btnImportSettings_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Title = "Select Settings File",
                InitialDirectory = @"C:\",
                Filter = "QuickPick settings (*.json) | *.json"
            };
            if (fileDialog.ShowDialog() == true)
            {
                var filePath = fileDialog.FileName;
                QP.SaveLoader.LoadSettingsFile(filePath);
            }
        }

        private void btnApplySettings_Click(object sender, RoutedEventArgs e)
        {
            QP.SaveLoader.SaveSettings();
        }

        private void btnSetTextOnButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = ((QpButton)lvButtons.SelectedItem);
                selectedItem.PredefinedText = txPredefText.Text;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        private void lvButtons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            stkPPredefText.Visibility = Visibility.Visible;
            var selectedItem = ((QpButton)lvButtons?.SelectedItem);
            txPredefText.Text = selectedItem?.PredefinedText ?? "";
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (QP.QuickPickModel.SettingsAreSaved == false)
            {
                var dialogResult = Forms.MessageBox.Show("Do you want to apply current settings?", "Apply settings?", Forms.MessageBoxButtons.YesNoCancel);
                if (dialogResult == Forms.DialogResult.Yes)
                {
                    QP.SaveLoader.SaveSettings();
                    this.Close();
                }
                else if (dialogResult == Forms.DialogResult.No)
                {
                    this.Close();
                }
                else if (dialogResult == Forms.DialogResult.Cancel)
                {
                  // do nothing.
                }
            }
            else
            {
                this.Close();
            }
        }
    }
}