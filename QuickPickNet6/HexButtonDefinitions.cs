using FontAwesome5;
using QuickPick.Logic;
using QuickPick.UI.BrightnessControls;
using QuickPick.UI.Views.Hex;
using QuickPick.UI.Views.Settings;
using QuickPick.Utilities;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

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
            IPercentageValueHandler brightnessControl = new BrightnessControl();
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


        public static void AsPlayPauseToggle(this HexagonButton button)
        {
            var playBackControl = new PlayBackControl();
            button.FontIcon = playBackControl.IsAudioPlaying ? EFontAwesomeIcon.Solid_Pause : EFontAwesomeIcon.Solid_Play;

            button.Hexagon.MouseDown += (sender, e) =>
            {
                InputSim.PlayPause();
                Dispatcher.CurrentDispatcher.Invoke(async () =>
                 {
                     await Task.Delay(500); // wait for the fade..
                     bool isAudioPlaying = playBackControl.IsAudioPlaying;
                     button.FontIcon = isAudioPlaying ? EFontAwesomeIcon.Solid_Pause : EFontAwesomeIcon.Solid_Play;
                 });
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
