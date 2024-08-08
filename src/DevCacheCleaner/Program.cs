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

        float reclaimedSpacesInMB = (reclaimedSpacesOfNuget + reclaimedSpacesOfXamarin) / 1024 / 1024;
        Console.WriteLine($"Reclaimed spaces: {reclaimedSpacesInMB:F2} MB");
    }
}

internal class Options
{
    [Option('d', "days", Required = false, Default = 30, HelpText = "Threshold days to delete old accessed caches")]
    public int ThresholdDays { get; set; }

    [Option("include-xamarin", Required = false, Default = false, HelpText = "Include Xamarin cache path")]
    public bool IncludeXamarin { get; set; }
}