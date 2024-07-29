namespace DevCacheCleaner.Nuget;

public class CachePathInfo
{
    public string HttpCache { get; }
    public string GlobalCache { get; }
    public string Temp { get; }

    public CachePathInfo(string httpCache, string globalCache, string temp)
    {
        this.HttpCache = httpCache;
        this.GlobalCache = globalCache;
        this.Temp = temp;
    }
}
