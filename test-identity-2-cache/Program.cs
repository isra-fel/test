// See https://aka.ms/new-console-template for more information


using Azure.Core;
using Azure.Identity;

var clientId = "1950a258-227b-4e31-a9cf-717495945fc2";
var tenantId = "72f988bf-86f1-41af-91ab-2d7cd011db47";
var authority = "https://login.microsoftonline.com";

var servicePrincipalAppId = ;
var servicePrincipalSecret = ;

{
    var appOptions = new ClientSecretCredentialOptions()
    {
        TokenCachePersistenceOptions = new TokenCachePersistenceOptions()
        {
            Name = "msalapp.cache"
        },
        AuthorityHost = new Uri(authority)
    };
    var appCredential = new ClientSecretCredential(tenantId, servicePrincipalAppId, servicePrincipalSecret, appOptions);
    var appToken = await appCredential.GetTokenAsync(new TokenRequestContext(new[] { "https://management.azure.com/.default" }));
    Console.WriteLine(appToken.Token);
}

{
    var userOptions = new InteractiveBrowserCredentialOptions()
    {
        ClientId = clientId,
        TenantId = tenantId,
        TokenCachePersistenceOptions = new TokenCachePersistenceOptions()
        {
            Name = "msaluser.cache"
        },
        AuthorityHost = new Uri(authority),
    };
    var browserCredential = new InteractiveBrowserCredential(userOptions);
    var userToken = await browserCredential.GetTokenAsync(new TokenRequestContext(new[] { "https://management.azure.com/.default" }));
    Console.WriteLine(userToken.Token);
}

// expected: app token cached in msalapp.cache, user token cached in msaluser.cache
// actuall: both tokens cached in msalapp.cache