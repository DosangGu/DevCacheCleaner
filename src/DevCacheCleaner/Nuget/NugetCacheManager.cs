using DevCacheCleaner.Nuget.GlobalPackage;
using DevCacheCleaner.Nuget.HttpCache;
using DevCacheCleaner.Shared;

namespace DevCacheCleaner.Nuget;

public class NugetCacheManager
{
    private CachePathInfo? _cachePathInfo;

    public NugetCacheManager()
    {
    }

    public async Task LoadCachePathInfoAsync(CancellationToken cancel = default)
    {
        if (_cachePathInfo == null)
        {
            this._cachePathInfo = await CLIHelper.GetCachePathInfoAsync(cancel);
        }
    }

    public long CleanCache(int thresholdDays)
    {
        if (this._cachePathInfo == null)
        {
            throw new InvalidOperationException("Should load cache path first.");
        }

        GlobalPackageManager globalPackageManager = new(this._cachePathInfo.GlobalCache);
        var recaimedSpacesOfGlobalPackageInBytes = globalPackageManager.CleanCache(thresholdDays);

        var httpCacheManager = new HttpCacheManager(_cachePathInfo.HttpCache);
        var recaimedSpacesOfHttpCacheInBytes = httpCacheManager.CleanCache();

        return recaimedSpacesOfGlobalPackageInBytes + recaimedSpacesOfHttpCacheInBytes;
    }
}
