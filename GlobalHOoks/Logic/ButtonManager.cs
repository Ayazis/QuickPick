using GlobalHOoks.Classes;
using GlobalHOoks.Enums;
using GlobalHOoks.Logic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GlobalHOoks
{
    public class ButtonManager
    {        
        private QuickPickModel _qpm;
        private WindowManager _windowManager;
        private ClickActions _clickActions;

        public ButtonManager(QuickPickModel quickPickModel, WindowManager windowManager)
        {
            
            _qpm = quickPickModel;
            this._windowManager = windowManager;
            _clickActions = new ClickActions(_windowManager);
        }


        internal void AddButtons()
        {
            var buttons = CreateButtons();
            var nrOfButtons = buttons.Count;
            double angle = 360 / (double)nrOfButtons;
            var i = 0;

            var style = WindowManager.ClickWindow.TryFindResource("RoundButton");
            foreach (var qpButton in buttons)
            {
                qpButton.Button.Content = qpButton.Id;

                qpButton.Margin = CalculateMargin(qpButton, qpButton.Button.Width, angle, i);
                qpButton.Button.Margin = qpButton.Margin;


                if (style != null)
                {
                    qpButton.Button.Style = style as Style;
                }

                _qpm.MainButtons.Add(qpButton);
                WindowManager.ClickWindow.Canvas.Children.Add(qpButton.Button);
                i++;
            }

            ConfigureButtons();
        }

        public List<QpButton> CreateButtons()
        {
            var buttons = new List<QpButton>();

            for (int i = 1; i <= _qpm.NrOfButtons; i++)
            {
                buttons.Add(new QpButton { Id = i });
            }
            return buttons;
        }

        private Thickness CalculateMargin(QpButton qpButton, double buttonWidth, double angle, int i, double radius = -1)
        {
            var cntr = _qpm.Center;

            if (radius == -1)
                radius = _qpm.CircleRadius;


            // angle gives angle Between each Button        
            var buttonAngle = (angle * i) - 90;
            var angleInRadians = (Math.PI / 180) * buttonAngle;

            double width = buttonWidth;
            double height = width;

            double x = (cntr.X - width / 2) + Math.Cos(angleInRadians) * radius;
            double y = (cntr.Y - height / 2) + Math.Sin(angleInRadians) * radius;

            return new Thickness(x, y, 0, 0);
        }

        private void ConfigureButtons()
        {
            var buttons = _qpm.MainButtons;
            var currentPath = AppDomain.CurrentDomain.BaseDirectory;
            var queryFiles = Directory.GetFiles(currentPath + @"Files\");

            for (int i = 1; i <= buttons.Count; i++)
            {
                var b = buttons[i - 1];
                if (i < 5)
                {
                    var queryToRun = queryFiles.FirstOrDefault(f => Path.GetFileName(f).StartsWith(b.Id.ToString()));
                    b.AssociatedFilePath = queryToRun;
                    b.Act = new QpButton.ActionDelegate(_clickActions.ReadAndRunQuery);
                    b.ActionType = ClickAction.RunQuery;
                }

                if (i == 6)
                {
                    b.ActionType = ClickAction.None;
                }

                if (i == 7)
                {
                    b.Act = new QpButton.ActionDelegate(_clickActions.TakeScreenSnip);
                    b.ActionType = ClickAction.TakeSnippet;
                }

                if (i == buttons.Count)
                {
                    b.Button.Content = 'X';
                    b.Act = new QpButton.ActionDelegate(_clickActions.CloseQuickPick);
                    b.ActionType = ClickAction.ExitQuickPick;
                }
            }
        }

        internal void AddShortCuts()
        {
            double angle = 360 / (double)_qpm.ShortCuts.Count;

            int i = 0;
            var withIcons = _qpm.ShortCuts.Where(w => w.Icon != null);
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
                    button.Act = new QpButton.ActionDelegate(_clickActions.LaunchApplication);

                    button.Icon.Width = button.Icon.Height = 25;
                    button.Icon.Margin = CalculateMargin(button, button.Icon.Width, angle, i, 100);
                    _qpm.ShortCutButtons.Add(button);
                    button.Icon.Visibility = Visibility.Hidden;
                    WindowManager.ClickWindow.Canvas.Children.Add(button.Icon);
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
                Logger.Log(ex);
                return null;
            }
        }

        internal void SetClickActionOnButton(QpButton button, ClickAction action)
        {
            if (action == ClickAction.ExitQuickPick)
            {
                button.Act = new QpButton.ActionDelegate(_clickActions.CloseQuickPick);
            }
            else if (action == ClickAction.RunProcess)
            {
                button.Act = new QpButton.ActionDelegate(_clickActions.LaunchApplication);
            }
            else if (action == ClickAction.RunQuery)
            {
                button.Act = new QpButton.ActionDelegate(_clickActions.ReadAndRunQuery);
            }
            else if (action == ClickAction.TakeSnippet)
            {
                button.Act = new QpButton.ActionDelegate(_clickActions.TakeScreenSnip);
            }
        }
    }
}

