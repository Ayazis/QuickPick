namespace UpdateInstaller
{
    public struct InstallerParams
    {
        public readonly string InstallerPath;
        public readonly InstallerArguments Arguments;

        public InstallerParams(string installerPath, InstallerArguments arguments)
        {
            InstallerPath = installerPath;
            Arguments = arguments;
        }
    }
}
