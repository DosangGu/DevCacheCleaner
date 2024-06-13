using DevCacheCleaner.Shared;

namespace DevCacheCleaner.Build;

internal class Project
{
    private string? _binPath;
    private string? _objPath;


    public string Path { get; }

    public Project(string path)
    {
        this.Path = path;
    }

    public void FindBuildArtifacts()
    {
        this._binPath = System.IO.Path.Combine(this.Path, "bin");
        this._objPath = System.IO.Path.Combine(this.Path, "obj");
    }

    public void CleanBuildArtifacts()
    {
        if (FileSystemHelper.IsFileOfDirectoryInUse(this._binPath!))
        {
            Console.WriteLine($"Skip deleting {this._binPath} because it is in use by another process.");
        }
        else
        {
            Directory.Delete(this._binPath!, true);
        }

        if (FileSystemHelper.IsFileOfDirectoryInUse(this._objPath!))
        {
            Console.WriteLine($"Skip deleting {this._objPath} because it is in use by another process.");
        }
        else
        {
            Directory.Delete(this._objPath!, true);
        }
    }
}
