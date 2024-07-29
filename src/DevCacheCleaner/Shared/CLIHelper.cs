using System.Diagnostics;

namespace DevCacheCleaner.Shared;

public static class CLIHelper
{
    public static async Task<CachePathInfo> GetCachePathInfoAsync(CancellationToken cancel = default)
    {
        var processInfo = new ProcessStartInfo("dotnet", $"nuget locals all --list")
        {
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        var process = new Process
        {
            StartInfo = processInfo
        };

        process.Start();
        Task<string> resultTask = process.StandardOutput.ReadToEndAsync(cancel);
        await process.WaitForExitAsync();

        var result = await resultTask;

        var httpCachePath = result.Split('\n').First(p => p.Contains("http-cache")).Split(':').Last().Trim();
        var globalCachePath = result.Split('\n').First(p => p.Contains("global-packages")).Split(':').Last().Trim();
        var tempPath = result.Split('\n').First(p => p.Contains("temp")).Split(':').Last().Trim();

        return new CachePathInfo(httpCachePath, globalCachePath, tempPath);
    }
}
