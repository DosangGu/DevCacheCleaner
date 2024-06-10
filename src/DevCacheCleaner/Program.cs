using CommandLine;
using DevCacheCleaner.Nuget;

namespace DevCacheCleaner;

internal class Program
{
    static void Main(string[] args)
    {
        var programOptions = Parser.Default.ParseArguments<Options>(args).Value;

        string nugetPackagesPath = NugetPathFinder.GetNugetPackagesPath(programOptions.NugetPackagesPath);

        List<NugetPackage> nugetPackages = Directory.GetDirectories(nugetPackagesPath)
            .Select(p =>
            {
                var package = new NugetPackage(p);
                package.LoadCachedVersions();
                return package;
            })
            .ToList();

        nugetPackages
            .ForEach(p =>
            {
                p.DeleteOldAccessedCaches(TimeSpan.FromDays(programOptions.ThresholdDays));
                p.DeleteSelfIfEmpty();
            });
    }
}

internal class Options
{
    [Option('d', "days", Required = false, Default = 30, HelpText = "Threshold days to delete old accessed caches")]
    public int ThresholdDays { get; set; }

    [Option("nuget-packages", Required = false, HelpText = "Nuget packages cache path to clean")]
    public string? NugetPackagesPath { get; set; }
}