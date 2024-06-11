using CommandLine;
using DevCacheCleaner.Nuget;

namespace DevCacheCleaner;

internal class Program
{
    static void Main(string[] args)
    {
        var programOptions = Parser.Default.ParseArguments<Options>(args).Value;

        var nugetCacheManager = new NugetCacheManager(programOptions.NugetPackagesPath);
        var recaimedSpacesOfNugetInBytes = nugetCacheManager.CleanCache(programOptions.ThresholdDays);

        Console.WriteLine($"Reclaimed spaces of nuget packages: {recaimedSpacesOfNugetInBytes} bytes");
    }
}

internal class Options
{
    [Option('d', "days", Required = false, Default = 30, HelpText = "Threshold days to delete old accessed caches")]
    public int ThresholdDays { get; set; }

    [Option("nuget-packages", Required = false, HelpText = "Nuget packages cache path to clean")]
    public string? NugetPackagesPath { get; set; }
}