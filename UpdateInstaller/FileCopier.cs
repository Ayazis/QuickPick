public static class FileCopier
{
    public static void CopyFiles(string sourceDirectory, string destinationDirectory)
    {
        // Create DirectoryInfo objects for the source and destination directories
        DirectoryInfo sourceDir = new DirectoryInfo(sourceDirectory);
        DirectoryInfo destDir = new DirectoryInfo(destinationDirectory);

        // Get all files in the source directory
        FileInfo[] files = sourceDir.GetFiles();

        // Copy each file to the destination directory
        foreach (FileInfo file in files)
        {
            string destFilePath = Path.Combine(destDir.FullName, file.Name);
            file.CopyTo(destFilePath, true); // Set overwrite to true to overwrite existing files
        }
    }

}