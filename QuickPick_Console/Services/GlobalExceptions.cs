using Ayazis.Utilities;
namespace QuickPick.Services;
internal interface IGlobalExceptions
{
    void SetupGlobalExceptionHandling();
}
internal class GlobalExceptions : IGlobalExceptions
{
    private ILogger _logger;

    public GlobalExceptions(ILogger logger)
    {
        _logger = logger;
    }
    public void SetupGlobalExceptionHandling()
    {
        // handle all exceptions
        AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
        {
            _logger.Log((Exception)e.ExceptionObject);
        };

        AppDomain.CurrentDomain.FirstChanceException += (sender, e) =>
        {
            _logger.Log(e.Exception);
        };
    }
}