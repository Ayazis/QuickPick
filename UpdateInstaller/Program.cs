using Updates;
using UpdateInstaller;
using UpdateInstaller.Updates;

public class Program
{
    public static void Main(string[] args)
    {
        // todo
        // Elevation check
        // some clever error handling.
        // logging
        InstallerArguments arguments = InstallerArguments.FromStringArray(args);
        UpdateManager updater = new UpdateManager(new ApplicationCloser());
        updater.CloseApplication(arguments.ProcessIdToKill);
        //dater.ExtractUpdateFile(arguments.SourceFolder, arguments.TargetFolder);
        updater.LaunchUpdatedApplication(arguments.PathToExecutable, arguments.TargetArguments);
    }


}