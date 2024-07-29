using CommandLine;
using DevCacheCleaner.Nuget;
using DevCacheCleaner.Shared;

namespace DevCacheCleaner;

internal class Program
{
    static async Task Main(string[] args)
    {
        var programOptions = Parser.Default.ParseArguments<Options>(args).Value;

        var cachePathInfo = await CLIHelper.GetCachePathInfoAsync();

        var nugetCacheManager = new GlobalCacheManager(cachePathInfo.GlobalCache);
        var recaimedSpacesOfNugetInBytes = nugetCacheManager.CleanCache(programOptions.ThresholdDays);

        float reclaimedSpacesOfNugetInMB = recaimedSpacesOfNugetInBytes / 1024 / 1024;
        Console.WriteLine($"Reclaimed spaces of nuget packages: {reclaimedSpacesOfNugetInMB:F2} MB");
    }
}

internal class Options
{
    [Option('d', "days", Required = false, Default = 30, HelpText = "Threshold days to delete old accessed caches")]
    public int ThresholdDays { get; set; }
}