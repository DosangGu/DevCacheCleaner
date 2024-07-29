namespace DevCacheCleaner.Nuget.GlobalPackage;

internal class GlobalPackageBundle
{
    public string DirectoryPath { get; }

    public IList<GlobalPackageVersion> CachedVersions { get; }

    private bool CanDeleteSelf
        => Directory.GetFiles(this.DirectoryPath).Length + Directory.GetDirectories(this.DirectoryPath).Length == 0;

    public GlobalPackageBundle(string path, string? name = default)
    {
        this.DirectoryPath = path;
        this.CachedVersions = [];
    }

    public void LoadCachedVersions()
    {
        this.CachedVersions.Clear();

        var versions = Directory
            .GetDirectories(this.DirectoryPath)
            .Select(q =>
            {
                var directoryInfo = new DirectoryInfo(q);
                return new GlobalPackageVersion(this, directoryInfo.Name);
            });

        foreach (var version in versions)
        {
            this.CachedVersions.Add(version);
        }
    }

    public void DeleteOldAccessedCaches(TimeSpan threshold)
    {
        var oldCaches = this.CachedVersions.Where(v => DateTime.Now - v.LastUsedAt > threshold);

        foreach (var cache in oldCaches)
        {
            cache.TryDelete();
        }
    }

    public void DeleteSelfIfEmpty()
    {
        if (this.CanDeleteSelf)
        {
            Directory.Delete(this.DirectoryPath);
        }
    }
}
