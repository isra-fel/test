using Azure.Containers.ContainerRegistry;
using Azure.Identity;

Uri endpoint = new Uri("https://***.azurecr.io");
ContainerRegistryClient client = new ContainerRegistryClient(endpoint, new AzurePowerShellCredential(),
    new ContainerRegistryClientOptions()
    {
        Audience = ContainerRegistryAudience.AzureResourceManagerPublicCloud
    });

Console.WriteLine(client.GetRepositoryNames().Count());
