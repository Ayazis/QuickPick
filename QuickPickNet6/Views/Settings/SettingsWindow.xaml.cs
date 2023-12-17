﻿using MouseAndKeyBoardHooks;
using System;
using System.Windows;
using System.Windows.Input;

namespace QuickPick.UI.Views.Settings
{
    /// <summary>
    /// Interaction logic for SettingsWindow4.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
		static SettingsWindow _instance;
		public static SettingsWindow Instance => _instance ??= new SettingsWindow();
		public SettingsViewModel ViewModel { get; private set; }

		private SettingsWindow()
        {
            ViewModel = new SettingsViewModel();
            this.DataContext = ViewModel;
            InitializeComponent();
            this.MouseLeftButtonDown += SettingsWindow_MouseLeftButtonDown;
        }

     

        public void ShowWindow()
        {
			var mousePosition = MousePosition.GetCursorPosition();
			Left = mousePosition.X - Width / 2;
			Top = mousePosition.Y - Height / 2;
			// get mouseposition.

			Show();
        }

        public event EventHandler ApplySettings;
        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            ApplySettings?.Invoke(this, EventArgs.Empty);
            this.Hide();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
     
        private void btnApplyNewCombo_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CurrentKeyCombo = ViewModel.NewKeyCombo;
            tbNewCombo.Visibility = Visibility.Collapsed;
            btnApplyNewCombo.Visibility = Visibility.Collapsed;
            btnCancelNewCombo.Visibility = Visibility.Collapsed;
            btnRecordNewCombo.Visibility = Visibility.Visible;
            this.KeyDown -= SettingsWindow_KeyDown;
            this.MouseDown -= SettingsWindow_MouseDown;
        }

        private void btnCancelNewCombo_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.NewKeyCombo = string.Empty;
            tbNewCombo.Visibility = Visibility.Collapsed;
            btnApplyNewCombo.Visibility = Visibility.Collapsed;
            btnCancelNewCombo.Visibility = Visibility.Collapsed;
            btnRecordNewCombo.Visibility = Visibility.Visible;
            this.KeyDown -= SettingsWindow_KeyDown;
            this.MouseDown -= SettingsWindow_MouseDown;
        }

        private void btnRecordNewCombo_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ClearNewKeyCombo();
            this.KeyDown += SettingsWindow_KeyDown;
            this.MouseDown += SettingsWindow_MouseDown;
            tbNewCombo.Visibility = Visibility.Visible;            
            btnApplyNewCombo.Visibility = Visibility.Visible;
            btnCancelNewCombo.Visibility = Visibility.Visible;
            btnRecordNewCombo.Visibility = Visibility.Collapsed;
        }
   
        private void SettingsWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void SettingsWindow_KeyDown(object sender, KeyEventArgs e)
        {
            Key key = e.Key;
            System.Windows.Forms.Keys formsKey = (System.Windows.Forms.Keys)KeyInterop.VirtualKeyFromKey(key);

            if (key == Key.System)
                formsKey = System.Windows.Forms.Keys.Alt;
            // todo:
            // Key system translates to Keys none
            ViewModel.AddKeyToNewCombo(formsKey);
            tbNewCombo.Text = ViewModel.NewKeyCombo;
        }
        private void SettingsWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MouseButton mouseButton = e.ChangedButton;
            System.Windows.Forms.Keys formsKey = System.Windows.Forms.Keys.None;

            switch (mouseButton)
            {
                // left mouse is not allowed.

                //case MouseButton.Left:
                //    formsKey = System.Windows.Forms.Keys.LButton; 
                //    break;
                case MouseButton.Right:
                    formsKey = System.Windows.Forms.Keys.RButton; 
                    break;
                case MouseButton.Middle:
                    formsKey = System.Windows.Forms.Keys.MButton;
                    break;
                case MouseButton.XButton1:
                    formsKey = System.Windows.Forms.Keys.XButton1; 
                    break;
                case MouseButton.XButton2:
                    formsKey = System.Windows.Forms.Keys.XButton2;
                    break;
            }

            ViewModel.AddKeyToNewCombo(formsKey);
            tbNewCombo.Text = ViewModel.NewKeyCombo;
            // Use formsKey as needed
        }

    }
}
