using System.Diagnostics;
using System.Reflection;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Extensions.Msal;

var clientAssertion = "real client assertion";
var cacheHelper = await MsalCacheHelper.CreateAsync(new StorageCreationPropertiesBuilder("msal.cache", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).Build());

var client = ConfidentialClientApplicationBuilder
    .Create("1950a258-227b-4e31-a9cf-717495945fc2")
    .WithTenantId("54826b22-38d6-4fb2-bad9-b7b93a3e9c5a")
    .WithClientId("3a7dbd02-25b9-4e69-bd83-c1b6f7c46636")
    .WithClientAssertion(clientAssertion)
    .Build();
cacheHelper.RegisterCache(client.AppTokenCache);

var result = client.AcquireTokenForClient(new[] { "https://management.core.windows.net//.default" })
    .ExecuteAsync()
    .Result;

Debug.WriteLine(result.AccessToken);

client = ConfidentialClientApplicationBuilder
    .Create("1950a258-227b-4e31-a9cf-717495945fc2")
    .WithTenantId("54826b22-38d6-4fb2-bad9-b7b93a3e9c5a")
    .WithClientId("3a7dbd02-25b9-4e69-bd83-c1b6f7c46636")
    .WithClientAssertion("invalid client assertion!")
    .Build();
cacheHelper.RegisterCache(client.AppTokenCache);

result = client.AcquireTokenForClient(new[] { "https://management.core.windows.net//.default" })
    .ExecuteAsync()
    .Result;

Debug.WriteLine(result.AccessToken);

