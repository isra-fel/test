// See https://aka.ms/new-console-template for more information

using class_lib;
using contract;

class DeploymentCreateCmdlet : DeploymentWhatIfCmdlet
{
    public override object GetDynamicParameters()
    {
        Console.WriteLine($"{nameof(DeploymentCreateCmdlet)}.GetDynamicParameters()");
        return base.GetDynamicParameters();
    }
}

abstract class DeploymentWhatIfCmdlet : ResourceWithParameterCmdletBase, IDynamicParameters
{

}

abstract class ResourceWithParameterCmdletBase: AzureRMCmdlet
{
    public virtual object GetDynamicParameters()
    {
        Console.WriteLine($"{nameof(ResourceWithParameterCmdletBase)}.GetDynamicParameters()");
        return nameof(ResourceWithParameterCmdletBase);
    }
}