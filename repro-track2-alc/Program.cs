using System.Reflection;
using System.Runtime.Loader;

namespace repro_track2_alc
{
    public class Program
    {
        static MyAlc myAlc;

        public static void Main(string[] args)
        {
            myAlc = new MyAlc();
            AssemblyLoadContext.Default.Resolving += Default_Resolving;
        }

        private static System.Reflection.Assembly Default_Resolving(AssemblyLoadContext context, System.Reflection.AssemblyName assemblyName)
        {
            return myAlc.LoadFromAssemblyName(assemblyName);
        }
    }

    class MyAlc : AssemblyLoadContext
    {
        protected override Assembly Load(AssemblyName assemblyName)
        {
            if (assemblyName.Name.Equals("Azure.Core"))
            {
                return LoadFromAssemblyPath(@"C:\Users\yeliu\Documents\PowerShell\Modules\Az.Accounts\2.7.4\AzSharedAlcAssemblies\Azure.Core.dll");
            }
            if (assemblyName.Name.Equals("System.Memory.Data"))
            {
                return LoadFromAssemblyPath(@"C:\Users\yeliu\Documents\PowerShell\Modules\Az.Accounts\2.7.4\AzSharedAlcAssemblies\System.Memory.Data.dll");
            }
            return null;
        }
    }
}
