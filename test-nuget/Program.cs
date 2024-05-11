// https://learn.microsoft.com/en-us/nuget/reference/nuget-client-sdk

internal class Program
{
    private static async Task Main(string[] args)
    {
        IEnumerable<Dependency> dependencies = new List<Dependency>() {
            // CreateAssembly("netcoreapp2.1", "Azure.Core", "1.31.0.0"),
        };

        foreach (Dependency dependency in dependencies)
        {
            try
            {
                Console.WriteLine($"Searching for package version... {dependency.AssemblyName}, {dependency.AssemblyVersion}, {dependency.TargetFramework}");
                var packageVersion = await GetPackageVersion(dependency.AssemblyName, dependency.AssemblyVersion, dependency.TargetFramework);
                dependency.PackageVersion = packageVersion;
                Console.WriteLine($"Found package version: {packageVersion}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed. Error: {ex.Message}");
            }
            finally
            {
                Console.WriteLine();
            }
        }

        Summarize(dependencies);
    }

    private static void Summarize(IEnumerable<Dependency> dependencies)
    {
        Console.WriteLine("Summary:");
        foreach (Dependency dependency in dependencies)
        {
            Console.WriteLine($"{dependency.AssemblyName}, {dependency.PackageVersion}");
        }
    }

    private static Dependency CreateAssembly(string targetFramework, string assemblyName, string assemblyVersion)
    {
        return new Dependency(assemblyName, new Version(assemblyVersion), targetFramework);
    }

    private static async Task<string> GetPackageVersion(string assemblyName, Version assemblyVersion, string targetFramework)
    {
        return await NugetHelper.GetPkgVerByAsmVerAsync(assemblyName, assemblyVersion, targetFramework);
    }
}

internal class Dependency
{
    public Dependency(string assemblyName, Version assemblyVersion, string targetFramework)
    {
        AssemblyName = assemblyName;
        AssemblyVersion = assemblyVersion;
        TargetFramework = targetFramework;
    }

    public string AssemblyName { get; set; }
    public Version AssemblyVersion { get; set; }
    public string TargetFramework { get; set; }
    public string? PackageVersion { get; set; }
}