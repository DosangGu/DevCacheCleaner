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

    public static long GetDirectorySize(string directoryPath)
    {
        long size = 0;

        DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
        FileInfo[] files = directoryInfo.GetFiles();

        foreach (FileInfo file in files)
        {
            size += file.Length;
        }

        DirectoryInfo[] directories = directoryInfo.GetDirectories();

        foreach (DirectoryInfo directory in directories)
        {
            size += GetDirectorySize(directory.FullName);
        }

        return size;
    }
}
