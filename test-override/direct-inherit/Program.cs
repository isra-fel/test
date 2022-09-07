// See https://aka.ms/new-console-template for more information

using contract;
using direct_inherit;

DirectInheritCmdlet c = new ();
if (c is IDynamicParameters dyna)
{
    Console.WriteLine(dyna.GetDynamicParameters());
}