using NuGet.Common;
using NuGet.Packaging;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

using System.Reflection;
using System.Runtime.InteropServices;

static class NugetHelper
{
    static SourceRepository repository = Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json");
    static SourceCacheContext cache = new SourceCacheContext();
    private static NuGetVersion targetPackageVersion = new NuGetVersion(0, 0, 0);

    static async Task<Version?> GetAsmVerByPkgVerAsync(string packageId, string packageVersion, string targetFramework)
    {
        Console.Write($"Trying {packageVersion}... ");
        ILogger logger = NullLogger.Instance;
        CancellationToken cancellationToken = CancellationToken.None;

        FindPackageByIdResource findResource = await repository.GetResourceAsync<FindPackageByIdResource>();

        using MemoryStream packageStream = new MemoryStream();
        await findResource.CopyNupkgToStreamAsync(
            packageId,
            new NuGetVersion(packageVersion),
            packageStream,
            cache,
            logger,
            cancellationToken);

        using PackageArchiveReader packageReader = new PackageArchiveReader(packageStream);

        // you can also get info about dependent packages by downloading the package and reading its nuspec file
        string targetAsmPathCompressed = $"lib/{targetFramework}/{packageId}.dll";
        if (!packageReader.GetFiles().Contains(targetAsmPathCompressed))
        {
            Console.WriteLine("not found");
            return null;
        }

        string targetAsmPath = Path.GetTempFileName();
        var entry = packageReader.GetEntry(targetAsmPathCompressed);
        using (var inputStream = entry.Open())
        using (var outputStream = File.OpenWrite(targetAsmPath))
        {
            inputStream.CopyTo(outputStream);
        }

        string[] runtimeAssemblies = Directory.GetFiles(RuntimeEnvironment.GetRuntimeDirectory(), "*.dll");
        var resolver = new PathAssemblyResolver(runtimeAssemblies);
        var mlc = new MetadataLoadContext(resolver);

        // todo: reuse mlc?
        using (mlc)
        using (var asmStream = File.OpenRead(targetAsmPath))
        {
            // Load assembly into MetadataLoadContext.
            Assembly assembly = mlc.LoadFromStream(asmStream);
            AssemblyName name = assembly.GetName();
            Console.WriteLine(name.Version);
            return name.Version;
        }
    }

    public static async Task<string> GetPkgVerByAsmVerAsync(string assemblyName, Version assemblyVersion, string targetFramework)
    {
        string packageId = assemblyName;
        List<NuGetVersion> availableVersions = new List<NuGetVersion>(await GetAvailableVersionsAsync(packageId));
        availableVersions.Sort((x, y) =>
        {
            // descending order
            return y.CompareTo(x);
        });
        return availableVersions.First(x =>
        {
            Version? asmVer = GetAsmVerByPkgVerAsync(packageId, x.ToFullString(), targetFramework).Result;
            return asmVer != null && asmVer.Equals(assemblyVersion);
        }).ToFullString();
    }

    async static Task<IEnumerable<NuGetVersion>> GetAvailableVersionsAsync(string packageId)
    {
        ILogger logger = NullLogger.Instance;
        CancellationToken cancellationToken = CancellationToken.None;

        FindPackageByIdResource findResource = await repository.GetResourceAsync<FindPackageByIdResource>();

        return await findResource.GetAllVersionsAsync(
            packageId,
            cache,
            logger,
            cancellationToken);
    }
}
