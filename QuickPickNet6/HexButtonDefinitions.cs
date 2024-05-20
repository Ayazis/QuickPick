using FontAwesome5;
using QuickPick.Logic;
using QuickPick.UI.BrightnessControls;
using QuickPick.UI.Views.Hex;
using QuickPick.UI.Views.Settings;
using QuickPick.Utilities;
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
            IValueHandler brightnessControl = new BrightnessControl();
            double startValue = 100;
            button.FontIcon = EFontAwesomeIcon.Solid_Adjust;
            var sliderControl = AddSliderControl(button, startValue);


            sliderControl.ValueChanged += (value) =>
            {
                brightnessControl.HandleNewValue(value);
            };
        }

        public static void AsVolumeControl(this HexagonButton button)
        {
            var volumeControl = new VolumeControl();

            button.FontIcon = EFontAwesomeIcon.Solid_VolumeUp;
            var sliderControl = AddSliderControl(button, volumeControl.CurrentVolume);
            sliderControl.ValueChanged += (value) =>
            {
                volumeControl.HandleNewValue(value);
            };
        }

        private static SliderUiControl AddSliderControl(HexagonButton button, double startValue)
        {
            var progressBar = CreateProgressbar();
            button.Grid.Children.Add(progressBar);
            progressBar.Value = startValue;            
            var sliderControl = new SliderUiControl(button, progressBar);
            sliderControl.AttachToButtonEvents();
            return sliderControl;
        }

        private static ProgressBar CreateProgressbar()
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
