using NAudio.CoreAudioApi;
using QuickPick.Logic;
using QuickPick.UI.Views.Settings;
using System;
using System.Windows.Controls;

using System.Windows.Input;
using System.Windows.Media;

using System.Windows.Shapes;

namespace QuickPick;

/// <summary>
/// Interaction logic for RingButtonControl.xaml
/// </summary>
public partial class RingButtonControl : UserControl
{
	MMDevice SoundDevice;
	public RingButtonControl()
	{
		var enumerator = new MMDeviceEnumerator();
		SoundDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);
		InitializeComponent();

		this.IsVisibleChanged += RingButtonControl_IsVisibleChanged;
	}

	private void RingButtonControl_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
	{
		if (IsAudioPlaying())
		{
			PlayButton.Visibility = System.Windows.Visibility.Collapsed;
			PauseButton.Visibility = System.Windows.Visibility.Visible;
		}
		else
		{
			PauseButton.Visibility = System.Windows.Visibility.Collapsed;
			PlayButton.Visibility = System.Windows.Visibility.Visible;
		}


		//	throw new System.NotImplementedException();
	}

	public bool IsAudioPlaying()
	{

		var count = SoundDevice.AudioMeterInformation.PeakValues.Count;
		for (int i = 0; i < count; i++)
		{
			float peakValue = SoundDevice.AudioMeterInformation.PeakValues[i];
			if (peakValue > 0)
			{
				return true;
			}
		}
		return false;
	}
	private void Path_MouseEnter(object sender, MouseEventArgs e)
	{
		//QuadrantEnter(sender as Path);
	}

	private void Path_MouseLeave(object sender, MouseEventArgs e)
	{
		//QuadrantLeave(sender as Path);
	}

	private void Path_MouseUp(object sender, MouseButtonEventArgs e)
	{
		Path source = sender as Path;
		if (source.Name == nameof(this.TopRight))
		{
			InputSim.CtrlAltBreak();
			ClickWindow.Instance.HideUI();
		}

	}

	private void QuadrantEnter(Path path)
	{
		path.Fill = Brushes.Black;
	}
	private void QuadrantLeave(Path path)
	{
		path.Fill = Brushes.Transparent;
	}

	private void SmallMiddleButton_Click(object sender, System.Windows.RoutedEventArgs e)
	{
		SettingsWindow.Instance.Show();
		SettingsWindow.Instance.Activate();
		SettingsWindow.Instance.Focus();


	}

	private void PlayButton_MouseDown(object sender, MouseButtonEventArgs e)
	{
		InputSim.PlayPause();
		TogglePlayAndPauseButtons();
	}

	private void PauseButton_MouseDown(object sender, MouseButtonEventArgs e)
	{
		InputSim.PlayPause();
		TogglePlayAndPauseButtons();
	}
	private void TogglePlayAndPauseButtons()
	{
		if (PlayButton.Visibility == System.Windows.Visibility.Visible)
		{
			PlayButton.Visibility = System.Windows.Visibility.Collapsed;
			PauseButton.Visibility = System.Windows.Visibility.Visible;
		}
		else
		{
			PauseButton.Visibility = System.Windows.Visibility.Collapsed;
			PlayButton.Visibility = System.Windows.Visibility.Visible;
		}

	}

	private void VolumeButton_MouseEnter(object sender, MouseEventArgs e)
	{
		ClickWindow.Instance.DisableMouseScroll();
		this.PreviewMouseWheel += RingButtonControl_PreviewMouseWheel;
	}

	private void RingButtonControl_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
	{
		if (VolumeBar.Visibility == System.Windows.Visibility.Collapsed)
			VolumeBar.Visibility = System.Windows.Visibility.Visible;

		bool scrollUp = e.Delta > 0;
		if (scrollUp)
		{
			InputSim.VolummeUp();
			UpdateVolumeBar();
		}
		else
		{
			InputSim.VolummeDown();
			UpdateVolumeBar();
		}
	}

	private void UpdateVolumeBar()
	{	
		MMDeviceEnumerator devEnum = new MMDeviceEnumerator();
		MMDevice defaultDevice = devEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
		float currentVolume = defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar * 100;

		VolumeBar.Value = currentVolume;
	}

	private void VolumeButton_MouseLeave(object sender, MouseEventArgs e)
	{
		VolumeBar.Visibility = System.Windows.Visibility.Collapsed;
		ClickWindow.Instance.EnableMouseScroll();
		this.PreviewMouseWheel -= RingButtonControl_PreviewMouseWheel;
	}

	private void VolumeButton_MouseDown(object sender, MouseButtonEventArgs e)
	{
		ToggleVolumeAndVolumeOffButtons();
		InputSim.ToggleMute();
	}
	private void VolumeOffButton_MouseDown(object sender, MouseButtonEventArgs e)
	{
		ToggleVolumeAndVolumeOffButtons();
		InputSim.ToggleMute();
	}

	private void ToggleVolumeAndVolumeOffButtons()
	{
		if (VolumeButton.Visibility == System.Windows.Visibility.Visible)
		{
			VolumeButton.Visibility = System.Windows.Visibility.Collapsed;
			VolumeOffButton.Visibility = System.Windows.Visibility.Visible;
		}
		else
		{
			VolumeOffButton.Visibility = System.Windows.Visibility.Collapsed;
			VolumeButton.Visibility = System.Windows.Visibility.Visible;
		}

	}


}
