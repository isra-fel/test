// https://learn.microsoft.com/en-us/nuget/reference/nuget-client-sdk

internal class Program
{
    private static async Task Main(string[] args)
    {
        // IEnumerable<Dependency> dependencies = new HardCodeAssemblyProvider().GetDependencies();
        IEnumerable<Dependency> dependencies = new FileSystemAssemblyProvider(@"C:\Users\yeliu\code\azure-powershell\src\lib\netstandard2.0", @"C:\Users\yeliu\code\azure-powershell\src\lib\netfx").GetDependencies();

        foreach (Dependency dependency in dependencies)
        {
            try
            {
                Console.WriteLine($"Searching for package version... {dependency.AssemblyName}, {dependency.AssemblyVersion}, {dependency.TargetFramework}");
                // var packageVersion = await GetPackageVersion(dependency.AssemblyName, dependency.AssemblyVersion, dependency.TargetFramework);
                // dependency.PackageVersion = packageVersion;
                // Console.WriteLine($"Found package version: {packageVersion}");
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

internal interface AssemblyProvider
{
    IEnumerable<Dependency> GetDependencies();
}

internal class HardCodeAssemblyProvider : AssemblyProvider
{
    public IEnumerable<Dependency> GetDependencies()
    {
        return new List<Dependency>() {
            CreateAssembly("netstandard2.0", "Azure.Core", "1.41.0.0"),
            CreateAssembly("netstandard2.0", "Azure.Identity", "1.12.0.0"),
            CreateAssembly("netstandard2.0", "Azure.Identity.Broker", "1.1.0.0"),
            CreateAssembly("netstandard2.0", "Microsoft.Bcl.AsyncInterfaces", "1.0.0.0"),
            CreateAssembly("netstandard2.0", "Microsoft.Identity.Client", "4.61.3.0"),
            CreateAssembly("netstandard2.0", "Microsoft.Identity.Client.Extensions.Msal", "4.61.3.0"),
            CreateAssembly("netstandard2.0", "Microsoft.Identity.Client.Broker", "4.61.3.0"),
            CreateAssembly("netstandard2.0", "Microsoft.Identity.Client.NativeInterop", "0.16.2.0"),
            CreateAssembly("netstandard2.0", "Microsoft.IdentityModel.Abstractions", "6.35.0.0"),
            CreateAssembly("netstandard2.0", "System.ClientModel", "1.0.0.0"),
            CreateAssembly("netstandard2.0", "System.Memory.Data", "1.0.2.0"),
            CreateAssembly("netstandard2.0", "System.Text.Json", "4.0.1.2"),

            CreateAssembly("netstandard2.0", "System.Buffers", "4.0.3.0"),
            CreateAssembly("netstandard2.0", "System.Memory", "4.0.1.1"),
            CreateAssembly("netstandard2.0", "System.Net.Http.WinHttpHandler", "4.0.4.0"),
            CreateAssembly("netstandard2.0", "System.Private.ServiceModel", "4.7.0.0"),
            CreateAssembly("netstandard2.0", "System.Security.AccessControl", "4.1.3.0"),
            CreateAssembly("netstandard2.0", "System.Security.Permissions", "4.0.3.0"),
            CreateAssembly("netstandard2.0", "System.Security.Principal.Windows", "4.1.3.0"),
            CreateAssembly("netstandard2.0", "System.ServiceModel.Primitives", "4.7.0.0"),
            CreateAssembly("netstandard2.0", "System.Threading.Tasks.Extensions", "4.2.0.1"),
            CreateAssembly("netfx", "Newtonsoft.Json", "13.0.0.0"),
            CreateAssembly("netfx", "System.Diagnostics.DiagnosticSource", "6.0.0.1"),
            CreateAssembly("netfx", "System.Numerics.Vectors", "4.1.4.0"),
            CreateAssembly("netfx", "System.Reflection.DispatchProxy", "4.0.4.0"),
            CreateAssembly("netfx", "System.Runtime.CompilerServices.Unsafe", "6.0.0.0"),
            CreateAssembly("netfx", "System.Security.Cryptography.Cng", "4.3.0.0"),
            CreateAssembly("netfx", "System.Security.Cryptography.ProtectedData", "4.5.0.0"),
            CreateAssembly("netfx", "System.Text.Encodings.Web", "4.0.5.1"),
            CreateAssembly("netfx", "System.Xml.ReaderWriter", "4.1.0.0")
        };
    }

    private static Dependency CreateAssembly(string targetFramework, string assemblyName, string assemblyVersion)
    {
        return new Dependency(assemblyName, new Version(assemblyVersion), targetFramework);
    }
}
