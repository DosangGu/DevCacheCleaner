using DevCacheCleaner.Shared;

namespace DevCacheCleaner.Nuget;

internal class NugetPackageVersion
{
    private readonly DirectoryInfo _directoryInfo;

    public NugetPackage NugetPackage { get; }

    public string Version { get; }

    public string DirectoryPath { get; }

    public DateTime LastUsed => this._directoryInfo.LastAccessTime;

    public NugetPackageVersion(NugetPackage nugetPackage, string version)
    {
        this.NugetPackage = nugetPackage;
        this.Version = version;
        this.DirectoryPath = Path.Combine(this.NugetPackage.DirectoryPath, this.Version);
        this._directoryInfo = new DirectoryInfo(this.DirectoryPath);
    }

    public bool IsInUsedByOtherProcess()
    {
        return FileSystemHelper.IsFileOfDirectoryInUse(this.DirectoryPath);
    }

    public bool TryDelete()
    {
        if (IsInUsedByOtherProcess() == false)
        {
            this._directoryInfo.Delete(true);
            return true;
        }
        else
            return false;
    }
}