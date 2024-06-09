namespace DevCacheCleaner;

internal class Program
{
    static void Main(string[] args)
    {
        string nugetPackagesPath = NugetPathFinder.GetNugetPackagesPath();

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
                p.DeleteOldAccessedCaches(TimeSpan.FromDays(30));
                p.DeleteSelfIfEmpty();
            });
    }
}
