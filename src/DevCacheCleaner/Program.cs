using CommandLine;
using DevCacheCleaner.Nuget;

namespace DevCacheCleaner;

internal class Program
{
    static void Main(string[] args)
    {
        var programOptions = Parser.Default.ParseArguments<Options>(args).Value;

        var nugetCacheManager = new NugetCacheManager();
        nugetCacheManager.LoadCachePathInfo();
        var reclaimedSpacesOfNuget = nugetCacheManager.CleanCache(programOptions.ThresholdDays);

        float reclaimedSpacesOfNugetInMB = reclaimedSpacesOfNuget / 1024 / 1024;
        Console.WriteLine($"Reclaimed spaces: {reclaimedSpacesOfNugetInMB:F2} MB");
    }
}

internal class Options
{
    [Option('d', "days", Required = false, Default = 30, HelpText = "Threshold days to delete old accessed caches")]
    public int ThresholdDays { get; set; }
}