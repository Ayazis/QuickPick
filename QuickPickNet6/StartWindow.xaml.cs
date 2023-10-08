using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Updates;

namespace QuickPick.UI;
/// <summary>
/// Interaction logic for StartWindow.xaml
/// </summary>
public partial class StartWindow : Window
{
    public StartWindow()
    {        
        InitializeComponent();
        ShowDownloadStartMessage();
    }

    public void ShowDownloadStartMessage()
    {
        UpdateUI(() =>
        {
            StatusTextBlock.Text = $"Hold for a minute while we download and install the latest verion for you...";
            DownloadProgressBar.Visibility = Visibility.Visible;
        });
    }
    public void Updater_DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
    {
        int newPercentage = (int)(e.BytesReceived / e.TotalBytesToReceive * 100);

        UpdateUI(() =>
        {
            DownloadProgressBar.Value = newPercentage;
        });
    }
    public void _updater_DownloadCompleted(object sender, EventArgs e)
    {
        UpdateUI(() =>
        {
            DownloadProgressBar.Value = 100;
            StatusTextBlock.Text = $"Installing version...";
        });
    }

    private void UpdateUI(Action action)
    {
        try
        {
            Dispatcher.Invoke(() =>
          {
              action();
          });
        }
        catch (Exception)
        {

            throw;
        }
    }
}

