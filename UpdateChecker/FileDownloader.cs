using System;
using System.Net;

namespace FileDownloader;

public interface IFileDownloader
{
    Task DownloadFilesAsync(Uri source, string destination);
    event EventHandler<DownloadProgressChangedEventArgs> DownloadProgressChanged;
}

public class FileDownloader : IFileDownloader
{
    public event EventHandler<DownloadProgressChangedEventArgs> DownloadProgressChanged;

    public Task DownloadFilesAsync(Uri source, string destination)
    {
        // Declare the TaskCompletionSource
        TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();

        WebClient webClient = new WebClient(); // using older webclient because it has built-in progress callback

        // Subscribe to DownloadProgressChanged to get progress updates
        webClient.DownloadProgressChanged += (s, e) =>
        {
            DownloadProgressChanged?.Invoke(s, e);
        };

        // Subscribe to DownloadFileCompleted to set the Task result
        webClient.DownloadFileCompleted += (s, e) =>
        {
            if (e.Cancelled)
            {
                tcs.SetCanceled();
            }
            else if (e.Error != null)
            {
                tcs.SetException(e.Error);
            }
            else
            {
                tcs.SetResult(null); // no need to do anything with the result. It is for awaiting the async call only.
            }
        };

        // Start the download
        webClient.DownloadFileAsync(source, destination); // returns void, not Task. Therefor not awaitable.

        // Return the Task to await
        return tcs.Task;
    }
}