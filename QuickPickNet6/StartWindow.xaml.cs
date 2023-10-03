using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QuickPick.UI;
/// <summary>
/// Interaction logic for StartWindow.xaml
/// </summary>
public partial class StartWindow : Window
{

	public StartWindow()
	{
		InitializeComponent();
		Loaded += MainWindow_Loaded;
	}

	private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
	{
		await CheckForUpdatesAsync();
	}

	private async Task CheckForUpdatesAsync()
	{
		// Simulate Checking for updates
		StatusTextBlock.Text = "Checking for update...";
		await Task.Delay(2000);  // Simulate network delay

		bool isUpdateAvailable = new Random().Next(0, 2) == 1;  // Simulate update availability

		if (isUpdateAvailable)
		{
			string version = "1.2.3";  // Simulate new version
			StatusTextBlock.Text = $"Version {version} available, downloading now...";
			DownloadProgressBar.Visibility = Visibility.Visible;

			// Simulate download with progress
			for (int i = 0; i <= 100; i++)
			{
				DownloadProgressBar.Value = i;
				await Task.Delay(50);  // Simulate network delay
			}

			DownloadProgressBar.Visibility = Visibility.Collapsed;
			StatusTextBlock.Text = "Download finished, restarting...";
			await Task.Delay(2000);  // Simulate delay before restarting

			// Restart logic here
		}
		else
		{
			StatusTextBlock.Text = "No update available, starting application...";
			await Task.Delay(2000);  // Simulate delay before starting application

			// Application start logic here
		}
	}
}

