using DevCacheCleaner.Shared;

namespace DevCacheCleaner.Nuget.GlobalPackage;

internal class GlobalPackageVersion
{
    private readonly DirectoryInfo _directoryInfo;

    public GlobalPackageBundle NugetPackage { get; }

    public string Version { get; }

    public string DirectoryPath { get; }

    public DateTime LastUsedAt { get; }

    public GlobalPackageVersion(GlobalPackageBundle nugetPackage, string version)
    {
        this.NugetPackage = nugetPackage;
        this.Version = version;
        this.DirectoryPath = Path.Combine(this.NugetPackage.DirectoryPath, this.Version);
        this._directoryInfo = new DirectoryInfo(this.DirectoryPath);
        this.LastUsedAt = FileSystemHelper.GetLastAccessedTimeOfDirectory(this._directoryInfo);
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