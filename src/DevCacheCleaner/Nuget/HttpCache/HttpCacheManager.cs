using DevCacheCleaner.Shared;

namespace DevCacheCleaner.Nuget.HttpCache;

internal class HttpCacheManager
{
    public string HttpCachePath { get; }

    public HttpCacheManager(string httpCachePath)
    {
        this.HttpCachePath = httpCachePath;
    }

    public long CleanCache()
    {
        var preVolume = FileSystemHelper.GetDirectorySize(this.HttpCachePath);

        var caches = Directory.GetFiles(this.HttpCachePath, "*.dat", SearchOption.AllDirectories);

        var expiredCaches = caches
            .Select(q => new FileInfo(q))
            .Where(q => q.LastAccessTime < DateTime.Now.AddMinutes(-30))
            .ToList();

        expiredCaches.ForEach(q => q.Delete());

        var postVolume = FileSystemHelper.GetDirectorySize(this.HttpCachePath);

        return preVolume - postVolume;
    }
}
