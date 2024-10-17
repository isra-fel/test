// https://learn.microsoft.com/en-us/nuget/reference/nuget-client-sdk





internal class FileSystemAssemblyProvider : AssemblyProvider
{
    private string[] Paths;

    public FileSystemAssemblyProvider(params string[] paths)
    {
        Paths = paths;
    }

    public IEnumerable<Dependency> GetDependencies()
    {
        return Paths.SelectMany(path => Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories))
            .Select(file => AssemblyMetadataHelper.GetAssemblyMetadata(file, GetTargetFrameworkFromPath(file)))
            .Where(metadata =>
            {
                if (metadata.Name != null && metadata.Version != null && metadata.TargetFramework != null)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine($"Skipping {metadata.Name}, {metadata.Version}, {metadata.TargetFramework}");
                    return false;
                }
            })
            .Select(metadata => new Dependency(metadata.Name!, metadata.Version!, metadata.TargetFramework!));
    }

    private string? GetTargetFrameworkFromPath(string file)
    {
        return Directory.GetParent(file)?.Name;
    }
}