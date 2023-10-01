using System;
using System.Linq;

public readonly struct InstallerArguments
{
	public string SourceFolder { get; }
	public string TargetFolder { get; }
	public int ProcessIdToKill { get; }
	public string PathToExecutable { get; }
	public string[] TargetArguments { get; }

	public InstallerArguments(string sourceFolder, string targetFolder, int processIdToKill, string pathToExecutable, string[] arguments)
	{
		SourceFolder = sourceFolder ?? throw new ArgumentNullException(nameof(sourceFolder));
		TargetFolder = targetFolder ?? throw new ArgumentNullException(nameof(targetFolder));
		ProcessIdToKill = processIdToKill;
		PathToExecutable = pathToExecutable ?? throw new ArgumentNullException(nameof(pathToExecutable));
		TargetArguments = arguments ?? Array.Empty<string>();
	}

	public static InstallerArguments FromStringArray(string[] args)
	{
		if (args == null || args.Length < 4)
		{
			throw new ArgumentException("Invalid number of arguments.");
		}

		if (args.Take(4).Any(arg => string.IsNullOrEmpty(arg)))
		{
			throw new ArgumentException("The first four arguments must not be null or empty.");
		}

		if (!int.TryParse(args[2], out int processIdToKill))
		{
			throw new ArgumentException("Process ID must be a valid integer.");
		}

		string[] additionalArgs = args.Skip(4).ToArray();
		return new InstallerArguments(args[0], args[1], processIdToKill, args[3], additionalArgs);
	}

	public override string ToString()
	{
		string basicArgs = $"{SourceFolder} {TargetFolder} {ProcessIdToKill} {PathToExecutable}";
		string additionalArgs = TargetArguments.Length > 0 ? string.Join(" ", TargetArguments.Select(arg => $"\"{arg}\"")) : string.Empty;
		return string.IsNullOrWhiteSpace(additionalArgs) ? basicArgs : $"{basicArgs} {additionalArgs}";
	}
}
