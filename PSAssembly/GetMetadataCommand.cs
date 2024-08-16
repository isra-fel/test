using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Runtime.InteropServices;

namespace PSAssembly
{
    [Cmdlet("Get", "AssemblyMetadata")]
    public class GetAssemblyMetadata : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string Path { get; set; }


        private MetadataLoadContext Mlc => _mlc.Value;
        private Lazy<MetadataLoadContext> _mlc = new Lazy<MetadataLoadContext>(() =>
        {
            //https://stackoverflow.com/questions/60191572/inspect-types-via-metadataloadcontext-fails-due-to-missing-assembly-net-core
            string[] runtimeAssemblies = Directory.GetFiles(RuntimeEnvironment.GetRuntimeDirectory(), "*.dll");
            var resolver = new PathAssemblyResolver(runtimeAssemblies);
            return new MetadataLoadContext(resolver);
        });

        protected override void ProcessRecord()
        {
            using (var asmStream = File.OpenRead(Path))
            {
                // Load assembly into MetadataLoadContext.
                Assembly assembly = Mlc.LoadFromStream(asmStream);
                AssemblyName name = assembly.GetName();

                IList<ANamespace> namespaces = new List<ANamespace>();
                foreach (var type in assembly.GetTypes())
                {
                    var namespaceName = type.Namespace;
                    var namespaceObj = namespaces.FirstOrDefault(n => n.Name == namespaceName);
                    if (namespaceObj == null)
                    {
                        namespaceObj = new ANamespace(namespaceName);
                        namespaces.Add(namespaceObj);
                    }
                    namespaceObj.Types.Add(new AType(name, type.Name, namespaceName));
                }
                WriteObject(new AAssembly(name) { Namespaces = namespaces });
            }
        }
    }

    public class AAssembly {
        public AssemblyName AssemblyName { get; set; }
        public IList<ANamespace> Namespaces { get; set; }
        public AAssembly(AssemblyName assemblyName)
        {
            AssemblyName = assemblyName;
            Namespaces = new List<ANamespace>();
        }
    }

    public class ANamespace
    {
        public IList<AType> Types { get; set; }
        public string Name { get; set; }
        public ANamespace(string name)
        {
            Name = name;
            Types = new List<AType>();
        }
    }
    public class AType
    {
        public AssemblyName AssemblyName { get; set; }
        public string Name { get; set; }
        public string NamespaceName { get; set; }
        public AType(AssemblyName assemblyName, string name, string namespaceName)
        {
            AssemblyName = assemblyName;
            Name = name;
            NamespaceName = namespaceName;
        }
    }
}
