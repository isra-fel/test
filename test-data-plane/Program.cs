using System.Net.Http.Headers;
using Azure.Core;
using Azure.Identity;
var cred = new AzurePowerShellCredential();


// {
//     var ctx = new TokenRequestContext(new[] { "https://vault.azure.net/.default" }); // conclusion 1: scope is audience + permission
//     var token = await cred.GetTokenAsync(ctx);
//     // Console.WriteLine(token.Token);

//     // make request to https://azurepsdev.vault.azure.net:443/secrets/bez-pat-for-ps-github-hook with the token
//     using var client = new HttpClient();
//     client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
//     var response = await client.GetAsync("https://azurepsdev.vault.azure.net:443/secrets/bez-pat-for-ps-github-hook?api-version=7.6");
//     var content = await response.Content.ReadAsStringAsync();
//     Console.WriteLine(content);
// }

{
    // Test storage blob
    var storageCtx = new TokenRequestContext(new[] { "https://yemingsa091701.blob.core.windows.net/.default" });
    //var storageCtx = new TokenRequestContext(new[] { "https://storage.azure.com/.default" }); // conclusion 2: both work
    var storageToken = await cred.GetTokenAsync(storageCtx);

    using var storageClient = new HttpClient();
    storageClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", storageToken.Token);
    storageClient.DefaultRequestHeaders.Add("x-ms-version", "2023-11-03");
    var storageResponse = await storageClient.GetAsync("https://yemingsa091701.blob.core.windows.net/ctn/a.txt");
    var storageContent = await storageResponse.Content.ReadAsStringAsync();
    Console.WriteLine(storageContent);
}