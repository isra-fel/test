This project is to test if loading a dependency assembly into a custom AssemblyLoadContext will affect the usage of the main assembly in the default ALC.

How to reproduce (make sure to update the paths in `Program.cs` and the following script):

```powershell
$DebugPreference = 'Continue'

function PrintAlc {
    [CmdletBinding()]
    param (
        [Parameter()]
        [string]
        $Assembly
    )
    [System.Runtime.Loader.AssemblyLoadContext]::All | foreach-object {
        write-host ($_.Name ?? "No Name")
        if ($Assembly -ne "") {
            $_.Assemblies | Where-Object -Property Location -iLike "*$Assembly*" | Sort-Object "Location" | Write-Output
        }
        else {
            $_.Assemblies | Sort-Object "Location" | Write-Output
        }
    }
}

cd .\code\test\repro-track2-alc\bin\Debug\netcoreapp2.1\
add-type -Path .\repro-track2-alc.dll

[repro_track2_alc.Program]::Main($null) # sets up ALC

printalc

add-type -Path "D:\Downloads\azure.security.keyvault.keys.4.3.0-beta.6\lib\netstandard2.0\Azure.Security.KeyVault.Keys.dll" # load key vault SDK into default ALC. Will not load Azure.Core until necessary.

printalc

$ass = [System.AppDomain]::CurrentDomain.GetAssemblies() | where {$_.FullName -ilike "*Azure.Security.KeyVault.Keys*"}
$ass.GetTypes() # will trigger the load of Azure.Core and load it into MyAlc

printalc
```
