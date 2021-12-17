// See https://aka.ms/new-console-template for more information

using contract;

DeploymentCreateCmdlet c = new DeploymentCreateCmdlet();
if (c is IDynamicParameters dyna)
{
    Console.WriteLine(dyna.GetDynamicParameters());
}