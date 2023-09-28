namespace QuickPick.Utilities.DesktopInterops
{
    public static class OsVersionChecker
    {
        public static bool IsWindows11Eligable => Environment.OSVersion.Version.Build >= 22000;
    }
}
