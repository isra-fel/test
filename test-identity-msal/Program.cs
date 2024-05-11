using Azure.Core;
using Azure.Identity;
using Azure.Identity.Broker;

class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Please enter a valid username and tenant ID.");
            return;
        }

        var clientId = "1950a258-227b-4e31-a9cf-717495945fc2";
        var username = args[0];
        var tenantId = args[1];
        var authority = "https://login.microsoftonline.com/";

        TokenCachePersistenceOptions tokenCachePersistenceOptions = new TokenCachePersistenceOptions()
        {
            UnsafeAllowUnencryptedStorage = true,
            Name = "msal.cache"
        };
        var wamOptions = new InteractiveBrowserCredentialBrokerOptions(WindowHandleUtilities.GetConsoleOrTerminalWindow())
        {
            IsLegacyMsaPassthroughEnabled = true,
            ClientId = clientId,
            TenantId = null,
            TokenCachePersistenceOptions = tokenCachePersistenceOptions,
            AuthorityHost = new Uri(authority),
        };
        var wamCredential = new InteractiveBrowserCredential(wamOptions);
        string[] scopes = ["https://management.core.windows.net//.default"];
        var wamToken = await wamCredential.GetTokenAsync(new TokenRequestContext(scopes));
        Console.WriteLine($"[InteractiveBrowserCredential] Got token from WAM: {wamToken.Token}");

        var silentOptions = new SharedTokenCacheCredentialBrokerOptions(tokenCachePersistenceOptions)
        {
            Username = username,
            IsLegacyMsaPassthroughEnabled = true,
            ClientId = clientId,
            TenantId = tenantId,
            AuthorityHost = new Uri(authority)
        };

        var silentCredential = new SharedTokenCacheCredential(silentOptions);
        var silentToken = await silentCredential.GetTokenAsync(new TokenRequestContext(scopes));
        Console.WriteLine($"[SharedTokenCacheCredential] Got token silently from WAM: {silentToken.Token}");
    }
}
