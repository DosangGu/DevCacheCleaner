using CommandLine;
using DevCacheCleaner.Nuget;
using DevCacheCleaner.Xamarin;

namespace DevCacheCleaner;

internal class Program
{
    static void Main(string[] args)
    {
        var programOptions = Parser.Default.ParseArguments<Options>(args).Value;

        var nugetCacheManager = new NugetCacheManager();
        nugetCacheManager.LoadCachePathInfo();
        var reclaimedSpacesOfNuget = nugetCacheManager.CleanCache(programOptions.ThresholdDays);

        var xamarinCacheManager = new XamarinCacheManager(programOptions.IncludeXamarin);
        var reclaimedSpacesOfXamarin = xamarinCacheManager.CleanCache();

        var normalizedReclaimedSpaces = GetNormalizedStringOfSize(reclaimedSpacesOfNuget + reclaimedSpacesOfXamarin);
        Console.WriteLine($"Reclaimed spaces: {normalizedReclaimedSpaces}");
    }

    private static string GetNormalizedStringOfSize(long bytes)
    {
        if (bytes < 1024)
        {
            return $"{bytes:F2} bytes";
        }
        else if (bytes < 1024 * 1024)
        {
            return $"{bytes / 1024:F2} KB";
        }
        else if (bytes < 1024 * 1024 * 1024)
        {
            return $"{bytes / 1024 / 1024:F2} MB";
        }
        else
        {
            return $"{bytes / 1024 / 1024 / 1024:F2} GB";
        }
    }
}

internal class Options
{
    [Option('d', "days", Required = false, Default = 30, HelpText = "Threshold days to delete old accessed caches")]
    public int ThresholdDays { get; set; }

    [Option("include-xamarin", Required = false, Default = false, HelpText = "Include Xamarin cache path")]
    public bool IncludeXamarin { get; set; }
}