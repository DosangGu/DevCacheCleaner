using DevCacheCleaner.Shared;

namespace DevCacheCleaner.Nuget;

internal class GlobalCacheManager
{
    public string GlobalCachePath { get; }

    public GlobalCacheManager(string globalCachePath)
    {
        this.GlobalCachePath = globalCachePath;
    }

    /// <summary>
    /// Clean nuget packages cache.
    /// </summary>
    /// <param name="thresholdDays">Threshold delete in days.</param>
    /// <returns>Recliamed spaces in bytes.</returns>
    public long CleanCache(int thresholdDays = 30)
    {
        var preVolume = FileSystemHelper.GetDirectorySize(this.GlobalCachePath);

        List<NugetPackage> nugetPackages = Directory.GetDirectories(this.GlobalCachePath)
            .Select(p =>
            {
                var package = new NugetPackage(p);
                package.LoadCachedVersions();
                return package;
            })
            .ToList();

        nugetPackages
            .ForEach(p =>
                {
                    p.DeleteOldAccessedCaches(TimeSpan.FromDays(thresholdDays));
                    p.DeleteSelfIfEmpty();
                });

        var postVolume = FileSystemHelper.GetDirectorySize(this.GlobalCachePath);

        return (preVolume - postVolume);
    }
}
