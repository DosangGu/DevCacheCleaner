namespace DevCacheCleaner.Nuget;

internal class NugetPackage
{
    public string Name { get; }

    public string DirectoryPath { get; }

    public IList<string> CachedVersions { get; }

    public bool IsDeleted { get; private set; }

    public NugetPackage(string path, string? name = default)
    {
        this.DirectoryPath = path;
        this.Name = name
            ?? Path.GetFileName(path) // HINT: Should parse directory name to get package name, but Path.GetFileName is enough for this example
            ?? throw new ArgumentException(name);
        this.CachedVersions = [];
        this.IsDeleted = false;
    }

    public void LoadCachedVersions()
    {
        this.CachedVersions.Clear();

        Directory.GetDirectories(this.DirectoryPath)
            .Select(q => Path.GetFileName(q))
            .ToList()
            .ForEach(v => this.CachedVersions.Add(v));
    }

    public void DeleteOldAccessedCaches(TimeSpan threshold)
    {
        this.CachedVersions
            .Select(v => new DirectoryInfo(Path.Combine(this.DirectoryPath, v)))
            .Where(d => DateTime.Now - d.LastAccessTime > threshold)
            .ToList()
            .ForEach(d => d.Delete(true));
    }

    public void DeleteSelfIfEmpty()
    {
        LoadCachedVersions();

        if (this.CachedVersions.Count == 0)
        {
            Directory.Delete(this.DirectoryPath);
            this.IsDeleted = true;
        }
    }
}
