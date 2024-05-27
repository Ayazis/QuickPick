using System.IO.Compression;

namespace UpdateInstaller.Updates;
public interface IArchiveExtractor
{
    void ExtractFiles(string sourceFile, string targetFolder);
}

public class ArchiveExtractor : IArchiveExtractor
{
    public void ExtractFiles(string sourceFile, string targetFolder)
    {
        CheckExtractionPrerequisites(sourceFile, targetFolder);

        try
        {
            ZipFile.ExtractToDirectory(sourceFile, targetFolder, true);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while extracting the archive: {ex.Message}", ex);
        }
    }

    private void CheckExtractionPrerequisites(string sourceFile, string targetFolder)
    {
        if (string.IsNullOrEmpty(sourceFile) || string.IsNullOrEmpty(targetFolder))
            throw new ArgumentException("Source file and target folder paths must not be null or empty.");


        if (!File.Exists(sourceFile))
            throw new FileNotFoundException($"The source file {sourceFile} does not exist.");


        if (!HasReadPermission(sourceFile))
            throw new UnauthorizedAccessException($"Insufficient permissions to read the file {sourceFile}.");


        if (!Directory.Exists(targetFolder))
            Directory.CreateDirectory(targetFolder);

        else if (!HasWritePermission(targetFolder))
            throw new UnauthorizedAccessException($"Insufficient permissions to write to the folder {targetFolder}.");
    }

    private bool HasReadPermission(string filePath)
    {
        try
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                // File was successfully opened for reading, close the stream and return true
            }
            return true;
        }
        catch (UnauthorizedAccessException)
        {
            // Access to the file was denied
            return false;
        }
        catch (Exception)
        {
            // Something ain't right.
            return false;
        }
    }
    private bool HasWritePermission(string folderPath)
    {
        string tempFilePath = Path.Combine(folderPath, "temp_permission_check.tmp");
        try
        {
            using (FileStream fs = new FileStream(tempFilePath, FileMode.CreateNew, FileAccess.Write))
            {
                // File was successfully created for writing, close the stream
            }
            File.Delete(tempFilePath);  // Remove the temporary file
            return true;
        }
        catch (UnauthorizedAccessException)
        {
            // Access to the folder was denied
            return false;
        }
        catch (Exception)
        {
            // Handle other exceptions if necessary
            return false;
        }
    }

}

