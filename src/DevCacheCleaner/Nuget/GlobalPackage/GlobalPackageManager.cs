using DevCacheCleaner.Shared;

namespace DevCacheCleaner.Nuget.GlobalPackage;

internal class GlobalPackageManager
{
    public string GlobalCachePath { get; }

    public GlobalPackageManager(string globalCachePath)
    {
        this.GlobalCachePath = globalCachePath;
    }

    /// <summary>
    /// Clean global packages cache.
    /// </summary>
    /// <param name="thresholdDays">Threshold delete in days.</param>
    /// <returns>Recliamed spaces in bytes.</returns>
    public long CleanCache(int thresholdDays = 30)
    {
        var preVolume = FileSystemHelper.GetDirectorySize(this.GlobalCachePath);

        List<GlobalPackageBundle> globalPackages = Directory.GetDirectories(this.GlobalCachePath)
            .Select(p =>
            {
                var package = new GlobalPackageBundle(p);
                package.LoadCachedVersions();
                return package;
            })
            .ToList();

        globalPackages
            .ForEach(p =>
                {
                    p.DeleteOldAccessedCaches(TimeSpan.FromDays(thresholdDays));
                    p.DeleteSelfIfEmpty();
                });

        var postVolume = FileSystemHelper.GetDirectorySize(this.GlobalCachePath);

        return preVolume - postVolume;
    }
}
