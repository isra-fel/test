using class_lib;
using contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace direct_inherit
{
    internal class DirectInheritCmdlet : AzureRMCmdlet, IDynamicParameters
    {
        public object GetDynamicParameters()
        {
            Console.WriteLine("Doing something in DirectInheritCmdlet class");
            return nameof(DirectInheritCmdlet);
        }
    }
}
