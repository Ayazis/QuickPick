using QuickPick.Classes;
using QuickPick.Enums;
using QuickPick.Logic;
using QuickPick.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace QuickPick
{
    public class ButtonManager
    {
        public Models.QuickPick QP { get; set; }

        public ButtonManager(Models.QuickPick QP)
        {
            this.QP = QP;
        }

        public void PlaceButtonsOnCanvas()
        {
            try
            {
                var buttons = QP.QuickPickModel.MainButtons;
                var nrOfButtons = buttons.Count;
                double angle = 360 / (double)nrOfButtons;
				int i = 0;
                var buttonStyle = QP.WindowManager.ClickWindow.TryFindResource("RoundButton");

                foreach (var qpButton in buttons)
                {
                    qpButton.Button.Content = qpButton.Id;

                    qpButton.Margin = CalculateMargin(qpButton, qpButton.Button.Width, angle, i);
                    qpButton.Button.Margin = qpButton.Margin;

                    if (buttonStyle != null)
                    {
                        qpButton.Button.Style = buttonStyle as Style;
                    }

                    QP.WindowManager.ClickWindow.Canvas.Children.Add(qpButton.Button);
                    i++;
                }
            }
            catch (Exception ex)
            {
                Logs.Logger.Log(ex);
            }

        }

        public void ClearCanvas()
        {
            try
            {
                var canvas = QP.WindowManager.ClickWindow.Canvas;
                canvas.Children.Clear();
            }
            catch (Exception ex)
            {
                Logs.Logger.Log(ex);
                
            }    
        }

        public void AddCentralButton()
        {
            var centralButton = new Button();
            double width = 25;

            centralButton.Width = width;
            centralButton.Height = width;
            centralButton.HorizontalAlignment = HorizontalAlignment.Center;
            centralButton.VerticalAlignment = VerticalAlignment.Center;
            centralButton.Click += QP.WindowManager.ClickWindow.btnShowShortCuts_Click;

            var buttonStyle = QP.WindowManager.ClickWindow.TryFindResource("RoundButton");
            if (buttonStyle != null)
                centralButton.Style = buttonStyle as Style;
            var center = QP.QuickPickModel.Center.X;
            centralButton.Margin = new Thickness( center - width / 2);

            var canvas = QP.WindowManager.ClickWindow.Canvas;
            canvas.Children.Add(centralButton);
        }

        private Thickness CalculateMargin(QpButton qpButton, double buttonWidth, double angle, int i, double radius = -1)
        {
            var cntr = QP.QuickPickModel.Center;

            if (radius == -1)
                radius = QP.QuickPickModel.CircleRadius;


            // angle gives angle Between each Button        
            var buttonAngle = (angle * i) - 90;
            var angleInRadians = (Math.PI / 180) * buttonAngle;

            double width = buttonWidth;
            double height = width;

            double x = (cntr.X - width / 2) + Math.Cos(angleInRadians) * radius;
            double y = (cntr.Y - height / 2) + Math.Sin(angleInRadians) * radius;

            return new Thickness(x, y, 0, 0);
        }

        /// <summary>
        /// Only used by the developer of QuickPick.
        /// </summary>
        private void ConfigureAyazisPreDefinedButtons()
        {
            var buttons = QP.QuickPickModel.MainButtons;
            var currentPath = AppDomain.CurrentDomain.BaseDirectory;
            var queryFiles = Directory.GetFiles(currentPath + @"Files\");

            for (int i = 1; i <= buttons.Count; i++)
            {
                var b = buttons[i - 1];
                if (i < 5)
                {
                    var queryToRun = queryFiles.FirstOrDefault(f => Path.GetFileName(f).StartsWith(b.Id.ToString()));
                    b.AssociatedFilePath = queryToRun;
                    b.Act = new QpButton.ActionDelegate(QP.ClickActions.ReadAndRunQuery);
                    b.ActionType = ClickAction.RunQuery;
                }

                if (i == 6)
                {

                    b.ActionType = ClickAction.None;
                    b.AssociatedFilePath = "";
                }

                if (i == 7)
                {

                    b.Act = new QpButton.ActionDelegate(QP.ClickActions.TakeScreenSnip);
                    b.ActionType = ClickAction.TakeSnippet;
                    b.AssociatedFilePath = "";
                }

                if (i == buttons.Count)
                {
                    b.Button.Content = 'X';
                    b.Act = new QpButton.ActionDelegate(QP.ClickActions.CloseQuickPick);
                    b.ActionType = ClickAction.ExitQuickPick;
                    b.AssociatedFilePath = "";
                }
            }
        }

        public QpButton ConfigureButton(QpButton button)
        {
            button.Button.ToolTip = button.FileName;

            if (button.ActionType == ClickAction.ExitQuickPick)
            {
                button.Act = new QpButton.ActionDelegate(QP.ClickActions.CloseQuickPick);
            }
            else if (button.ActionType == ClickAction.RunProcess)
            {
                button.Act = new QpButton.ActionDelegate(QP.ClickActions.LaunchApplication);
            }
            else if (button.ActionType == ClickAction.RunQuery)
            {
                button.Act = new QpButton.ActionDelegate(QP.ClickActions.ReadAndRunQuery);
            }
            else if (button.ActionType == ClickAction.TakeSnippet)
            {
                button.Act = new QpButton.ActionDelegate(QP.ClickActions.TakeScreenSnip);
            }
            else if(button.ActionType == ClickAction.PasteText)
            {
                button.Act = new QpButton.ActionDelegate(QP.ClickActions.PasteText);
            }

            return button;
        }

        public void AddShortCuts()
        {
            double angle = 360 / (double)QP.QuickPickModel.ShortCuts.Count;

            int i = 0;
            var withIcons = QP.QuickPickModel.ShortCuts.Where(w => w.Icon != null);
            foreach (var shortcut in withIcons)
            {
                try
                {
                    i++;
                    var button = new QpButton();
                    button.AssociatedFilePath = shortcut.TargetPath;

                    if (shortcut.Icon != null)
                    {
                        button.Icon.Source = ToImage(shortcut.Icon);
                        button.IconLocation = shortcut.IconLocation;
                    }
                    button.ActionType = ClickAction.RunProcess;
                    button.Act = new QpButton.ActionDelegate(QP.ClickActions.LaunchApplication);

                    button.Icon.Width = button.Icon.Height = 25;
                    button.Icon.Margin = CalculateMargin(button, button.Icon.Width, angle, i, 100);
                    QP.QuickPickModel.ShortCutButtons.Add(button);
                    button.Icon.Visibility = Visibility.Hidden;
                    QP.WindowManager.ClickWindow.Canvas.Children.Add(button.Icon);
                }
                catch (Exception)
                {

                }
            }
        }

        public ImageSource ToImage(Icon icon)
        {
            try
            {
                ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                return imageSource;
            }
            catch (Exception ex)
            {
                Logs.Logger.Log(ex);
                return null;
            }
        }

        public void SetClickActionOnButton(QpButton button, ClickAction action)
        {
            if (action == ClickAction.ExitQuickPick)
            {
                button.Act = new QpButton.ActionDelegate(QP.ClickActions.CloseQuickPick);
            }
            else if (action == ClickAction.RunProcess)
            {
                button.Act = new QpButton.ActionDelegate(QP.ClickActions.LaunchApplication);
            }
            else if (action == ClickAction.RunQuery)
            {
                button.Act = new QpButton.ActionDelegate(QP.ClickActions.ReadAndRunQuery);
            }
            else if (action == ClickAction.TakeSnippet)
            {
                button.Act = new QpButton.ActionDelegate(QP.ClickActions.TakeScreenSnip);
            }
        }

    }
}