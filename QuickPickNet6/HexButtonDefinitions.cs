using FontAwesome5;
using QuickPick.Logic;
using QuickPick.UI.BrightnessControls;
using QuickPick.UI.Views.Hex;
using QuickPick.UI.Views.Settings;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            var sliderControl = AddSliderControl(button);
            var brightnessControl = new BrightnessControl();

            sliderControl.ValueChanged += (value) =>
            {
                brightnessControl.SetBrightnessOnAllScreens((int)value);
            };
        }

        public static void AsVolumeControl(this HexagonButton button)
        {
            button.FontIcon = EFontAwesomeIcon.Solid_VolumeUp;
            var sliderControl = AddSliderControl(button);            

            sliderControl.ValueChanged += (value) =>
            {
                brightnessControl.SetBrightnessOnAllScreens((int)value);
            };
        }



        private static SliderControl AddSliderControl(HexagonButton button)
        {
            var progressBar = AddProgressBar(button);
            button.Grid.Children.Add(progressBar);
            var sliderControl = new SliderControl(button, progressBar);
            sliderControl.AttachToButtonEvents();
            return sliderControl;
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
