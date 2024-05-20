using FontAwesome5;
using QuickPick.Logic;
using QuickPick.UI.Views.Hex;
using QuickPick.UI.Views.Settings;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;

namespace QuickPick
{
    public static class HexButtonDefinitions
    {
        public static void AsSettingsButton(this HexagonButton button)
        {
            button.FontIcon = EFontAwesomeIcon.Solid_Tools;
            button.Hexagon.MouseDown += (sender, e) =>
            {
                SettingsWindow.Instance.ShowWindow();
                SettingsWindow.Instance.Activate();
                SettingsWindow.Instance.Focus();
                ClickWindow.Instance.HideUI();
            };
        }

        public static void AsShowDesktopButton(this HexagonButton button)
        {
            button.FontIcon = EFontAwesomeIcon.Solid_Desktop;
            button.Hexagon.MouseDown += (sender, e) =>
            {
                InputSim.WinD();
                ClickWindow.Instance.HideUI();
            };
        }

        public static void AsBrightnessControl(this HexagonButton button)
        {
            button.FontIcon = EFontAwesomeIcon.Solid_Adjust;
            AddProgressControl(button);
        }

        private static void AddProgressControl(HexagonButton button)
        {
            var progressBar = AddProgressBar(button);
            button.Grid.Children.Add(progressBar);

            button.Hexagon.PreviewMouseDown += (sender, e) =>
            {

            };

            button.Hexagon.PreviewMouseUp += (sender, e) =>
            {

            };

            button.Hexagon.PreviewMouseMove += (sender, e) =>
            {

            };


        }

        private static void Hexagon_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private static ProgressBar AddProgressBar(HexagonButton button)
        {
            // Create the ProgressBar
            ProgressBar progressBar = new ProgressBar
            {
                Name = "BrightnessBar",
                Orientation = Orientation.Vertical,
                Width = 5,
                Height = 50,
                Margin = new Thickness(6, -7, 23, 0),
                VerticalAlignment = VerticalAlignment.Top,
                Visibility = Visibility.Collapsed
            };

            return progressBar;
        }
    }
}
