using System.Net;

namespace Updates;

public interface IFileDownloader
{
    void DownloadFile(Uri source);
    event EventHandler<DownloadProgressChangedEventArgs> DownloadProgressChanged;
}

public class FileDownloader : IFileDownloader
{
	private string _destinationFolder;

	public FileDownloader(string destinationFolder)
	{
		this._destinationFolder = destinationFolder;
        CreateDestinationFolderIfNotExists();
	}
	void CreateDestinationFolderIfNotExists()
	{
		if(!Directory.Exists(this._destinationFolder)) 
        {
            Directory.CreateDirectory(this._destinationFolder); 
        }
	}

	public event EventHandler<DownloadProgressChangedEventArgs> DownloadProgressChanged;

    public void DownloadFile(Uri source)
    {
        // Declare the TaskCompletionSource
        TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();

        WebClient webClient = new WebClient(); // using older webclient because it has built-in progress callback

        //// Subscribe to DownloadProgressChanged to get progress updates
        //webClient.DownloadProgressChanged += (s, e) =>
        //{
        //    DownloadProgressChanged?.Invoke(s, e);
        //};

        //// Subscribe to DownloadFileCompleted to set the Task result
        //webClient.DownloadFileCompleted += (s, e) =>
        //{
        //    if (e.Cancelled)
        //    {
        //        tcs.SetCanceled();
        //    }
        //    else if (e.Error != null)
        //    {
        //        tcs.SetException(e.Error);
        //    }
        //    else
        //    {
        //        tcs.SetResult(null); // no need to do anything with the result. It is for awaiting the async call only.
        //    }
        //};

        string fileName = Path.GetFileName(source.AbsolutePath);
        string destination = Path.Join(this._destinationFolder, fileName);

        // Start the download
        webClient.DownloadFile(source, destination); // returns void, not Task. Therefor not awaitable.

        // Return the Task to await
        //return tcs.Task;
    }
}