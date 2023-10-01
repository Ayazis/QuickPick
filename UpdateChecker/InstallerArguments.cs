public readonly struct InstallerArguments
{
	public string SourceFolder { get; }
	public string TargetFolder { get; }
	public int ProcessIdToKill { get; }
	public string PathToExecutable { get; }

	private InstallerArguments(string sourceFolder, string targetFolder, int processIdToKill, string pathToExecutable)
	{
		SourceFolder = sourceFolder;
		TargetFolder = targetFolder;
		ProcessIdToKill = processIdToKill;
		PathToExecutable = pathToExecutable;
	}

	public static InstallerArguments FromStringArray(string[] args)
	{
		if (args == null || args.Length != 4)
		{
			throw new ArgumentException("Invalid number of arguments.");
		}

		if (args.Any(arg => string.IsNullOrEmpty(arg)))
		{
			throw new ArgumentException("Arguments must not be null or empty.");
		}

		if (!int.TryParse(args[2], out int processIdToKill))
		{
			throw new ArgumentException("Process ID must be a valid integer.");
		}

		return new InstallerArguments(args[0], args[1], processIdToKill, args[3]);
	}
}
