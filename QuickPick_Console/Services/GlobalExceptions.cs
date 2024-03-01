using Ayazis.Utilities;
namespace QuickPick.Services;
internal interface IGlobalExceptions
{
    void SetupGlobalExceptionHandling();
}
internal class GlobalExceptions : IGlobalExceptions
{
    public void SetupGlobalExceptionHandling()
    {
        // handle all exceptions
        AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
        {
            Logs.Logger?.Log((Exception)e.ExceptionObject);
        };

        AppDomain.CurrentDomain.FirstChanceException += (sender, e) =>
        {
            Logs.Logger?.Log(e.Exception);
        };
    }
}
