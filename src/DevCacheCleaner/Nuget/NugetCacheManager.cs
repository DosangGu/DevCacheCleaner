using DevCacheCleaner.Shared;

namespace DevCacheCleaner.Nuget;

internal class NugetCacheManager
{
    private string NugetPackagesPath => string.IsNullOrEmpty(this.OverridedNugetPackagesPath)
        ? GetSystemNugetPackagesPath()
        : this.OverridedNugetPackagesPath!;

    public string? OverridedNugetPackagesPath { get; }

    public NugetCacheManager(string? overridedNugetPackagesPath = default)
    {
        this.OverridedNugetPackagesPath = overridedNugetPackagesPath;
    }

    /// <summary>
    /// Clean nuget packages cache.
    /// </summary>
    /// <param name="thresholdDays">Threshold delete in days.</param>
    /// <returns>Recliamed spaces in bytes.</returns>
    public long CleanCache(int thresholdDays)
    {
        var preVolume = FileSystemHelper.GetDirectorySize(this.NugetPackagesPath);

        List<NugetPackage> nugetPackages = Directory.GetDirectories(this.NugetPackagesPath)
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

        var postVolume = FileSystemHelper.GetDirectorySize(this.NugetPackagesPath);

        return (preVolume - postVolume);
    }

    private static string GetSystemNugetPackagesPath()
    {
        string? customNugetPath = Environment.GetEnvironmentVariable("NUGET_PACKAGES");
        if (customNugetPath is not null)
        {
            return customNugetPath;
        }
        else
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                ".nuget",
                "packages"
            );
        }
    }
}
