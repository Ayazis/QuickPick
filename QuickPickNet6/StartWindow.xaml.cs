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
    Version _newVersion;
    UpdateDownloader _updater;
    public StartWindow()
    {
        InitializeComponent();
    }

    public async Task<string> StartDownloadAsync(UpdateDownloader updater)
    {
        _updater = updater;
        _newVersion = await _updater.GetLatestVersionAsync();

        UpdateUI(() =>
        {
            StatusTextBlock.Text = $"Downloading version {_newVersion}...";
            DownloadProgressBar.Visibility = Visibility.Visible;
        });

        _updater.DownloadProgressChanged += Updater_DownloadProgressChanged;
        _updater.DownloadCompleted += _updater_DownloadCompleted;
        string downloadResult = await _updater.DownloadUpdateAsync();

        await Task.Delay(200); // give time for the ui to display 100%.
        UpdateUI(Close);

        return downloadResult;
    }
    private void Updater_DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
    {
        int newPercentage = (int)(e.BytesReceived / e.TotalBytesToReceive * 100);

        UpdateUI(() =>
        {
            DownloadProgressBar.Value = newPercentage;
        });
    }
    private void _updater_DownloadCompleted(object sender, EventArgs e)
    {

        _updater.DownloadCompleted -= _updater_DownloadCompleted;
        _updater.DownloadProgressChanged -= Updater_DownloadProgressChanged;

        UpdateUI(() =>
        {
            DownloadProgressBar.Value = 100;
            StatusTextBlock.Text = $"Installing version {_newVersion}...";
        });
    }

    public void UpdateUI(Action action)
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

