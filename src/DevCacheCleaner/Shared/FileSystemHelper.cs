namespace DevCacheCleaner.Shared;

internal static class FileSystemHelper
{
    public static bool IsFileOfDirectoryInUse(string directoryPath)
    {
        var files = Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            try
            {
                using var _ = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
        }
        return false;
    }
}
