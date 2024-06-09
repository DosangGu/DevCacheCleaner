namespace DevCacheCleaner;

internal static class NugetPathFinder
{
    public static string GetNugetPackagesPath(string? overridedNugetPackagesPath = default)
    {
        string? customNugetPath = Environment.GetEnvironmentVariable("NUGET_PACKAGES");
        if (customNugetPath is not null)
        {
            return customNugetPath;
        }
        else
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                ".nuget",
                "packages"
            );
        }
    }
}
