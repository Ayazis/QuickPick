using System.Diagnostics;

namespace InstallerExecutableLauncher;

public interface IInstallerLauncher
{
    void LaunchInstaller(string installerPath, string[] arguments);
}


public class InstallerLauncher : IInstallerLauncher
{
    public void LaunchInstaller(string installerPath, string[] arguments)
    {
        // Validate installer path
        if (string.IsNullOrEmpty(installerPath))
            throw new ArgumentException("Installer path must not be null or empty.");


        if (!File.Exists(installerPath))
            throw new FileNotFoundException($"The installer file does not exist: {installerPath}");


        // Prepare arguments
        string args = arguments != null ? string.Join(" ", arguments.Select(a => $"\"{a}\"")) : string.Empty;

        try
        {
            // Launch installer
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = installerPath,
                Arguments = args
            };

            Process.Start(startInfo);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to launch installer: {ex.Message}", ex);
        }
    }
}