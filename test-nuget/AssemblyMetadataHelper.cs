using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

internal class AssemblyMetadataHelper
{
    public static AssemblyMetadata GetAssemblyMetadata(string path, string? overrideTargetFramework = null)
    {
        string[] runtimeAssemblies = Directory.GetFiles(RuntimeEnvironment.GetRuntimeDirectory(), "*.dll");
        var resolver = new PathAssemblyResolver(runtimeAssemblies);
        var mlc = new MetadataLoadContext(resolver);

        // todo: reuse mlc?
        using (mlc)
        using (var asmStream = File.OpenRead(path))
        {
            try
            {
                Assembly assembly = mlc.LoadFromStream(asmStream);
                AssemblyName name = assembly.GetName();
                return new AssemblyMetadata()
                {
                    Name = name?.Name,
                    Version = name?.Version,
                    TargetFramework = overrideTargetFramework ?? GetTargetFramework(assembly)
                };
            }
            catch (BadImageFormatException e)
            {
                Console.WriteLine($"Skipping {path} due to {e.Message}");
                return new AssemblyMetadata()
                {
                    Name = path,
                    Version = null,
                    TargetFramework = overrideTargetFramework
                };
            }
        }
    }

    private static string? GetTargetFramework(Assembly assembly)
    {
        return assembly.GetCustomAttributesData()
            .FirstOrDefault(x => x.AttributeType.ToString() == typeof(TargetFrameworkAttribute).ToString())
            ?.ConstructorArguments[0].Value?.ToString();
    }

    // private static IDictionary<string, string> HardCodedTargetFramework = new Dictionary<string, string>() {
    //     {"Microsoft.Bcl.AsyncInterfaces", ".NETStandard,Version=v2.0"},
    //     {"System.Buffers", ".NETStandard,Version=v2.0"}
    // };
}

class AssemblyMetadata
{
    public string? Name { get; set; }
    public Version? Version { get; set; }
    public string? TargetFramework { get; set; }
}