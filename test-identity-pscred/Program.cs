// See https://aka.ms/new-console-template for more information
using Azure.Core;
using Azure.Identity;

Console.WriteLine(await new AzurePowerShellCredential().GetTokenAsync(new TokenRequestContext(scopes: ["api:///63beacf8-d8af-49f9-a8ad-07651b2519cb"])));
// Console.WriteLine(await new AzurePowerShellCredential().GetTokenAsync(new TokenRequestContext(scopes: ["https://management.azure.com/.default"])));
// Console.WriteLine(await new AzurePowerShellCredential().GetTokenAsync(new TokenRequestContext(scopes: ["https://management.azure.com/.default", "api:///63beacf8-d8af-49f9-a8ad-07651b2519cb"])));
