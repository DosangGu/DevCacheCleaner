using System.Diagnostics.CodeAnalysis;
using DevCacheCleaner.Shared;

namespace DevCacheCleaner.Xamarin;

internal class XamarinCacheManager
{
    private readonly bool _isActivated;
    private readonly string? _xamarinBuildDownloadCachePath;
    private readonly string? _xamarinCachePath;

    [MemberNotNullWhen(true, nameof(_xamarinBuildDownloadCachePath))]
    private bool IsSupported => this._isActivated && this._xamarinBuildDownloadCachePath != null;

    public XamarinCacheManager(bool isActivated)
    {
        this._isActivated = isActivated;
        if (OperatingSystem.IsOSPlatform("WINDOWS"))
        {
            var appDataLocal = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            this._xamarinBuildDownloadCachePath = Path.Combine(appDataLocal, "XamarinBuildDownloadCache");
        }
        else if (OperatingSystem.IsOSPlatform("MACOS"))
        {
            var userDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            this._xamarinBuildDownloadCachePath = Path.Combine(userDir, "Library", "Caches", "XamarinBuildDownload");
            this._xamarinCachePath = Path.Combine(userDir, "Library", "Caches", "Xamarin");
        }
        else
        {
        }
    }

    public long CleanCache()
    {
        if (this.IsSupported == false)
        {
            return 0;
        }

        var preVolume = FileSystemHelper.GetDirectorySize(this._xamarinBuildDownloadCachePath);
        preVolume += this._xamarinCachePath != null ? FileSystemHelper.GetDirectorySize(this._xamarinCachePath) : 0;

        var xamarinBuildDownloadCacheDirInfo = new DirectoryInfo(this._xamarinBuildDownloadCachePath);
        if (xamarinBuildDownloadCacheDirInfo.Exists)
            xamarinBuildDownloadCacheDirInfo.Delete(true);

        if (this._xamarinCachePath != null)
        {
            var xamarinCacheDirInfo = new DirectoryInfo(this._xamarinCachePath);
            if (xamarinCacheDirInfo.Exists)
                xamarinCacheDirInfo.Delete(true);
        }

        var postVolume = FileSystemHelper.GetDirectorySize(this._xamarinBuildDownloadCachePath);
        postVolume += this._xamarinCachePath != null ? FileSystemHelper.GetDirectorySize(this._xamarinCachePath) : 0;

        return preVolume - postVolume;
    }
}
