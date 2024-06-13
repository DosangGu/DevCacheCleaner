using Microsoft.Build.Construction;

namespace DevCacheCleaner.Build;

internal class Solution
{
    private readonly List<Project> _projects;

    public string Path { get; }

    public IReadOnlyList<Project> Projects => this._projects;

    public Solution(string path)
    {
        this.Path = path;
        this._projects = [];
    }

    public void FindProjects()
    {
        var solutionFile = SolutionFile.Parse(this.Path);
        var projects = solutionFile.ProjectsInOrder
            .Where(p => p.ProjectType == SolutionProjectType.KnownToBeMSBuildFormat)
            .Select(p => new Project(p.AbsolutePath));

        foreach (var project in projects)
        {
            this._projects.Add(project);
        }
    }

    public void CleanBuildArtifacts()
    {
        foreach (var project in this._projects)
        {
            project.FindBuildArtifacts();
            project.CleanBuildArtifacts();
        }
    }
}
