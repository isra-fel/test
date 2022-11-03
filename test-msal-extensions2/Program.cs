using System;
using System.IO;
using Microsoft.Identity.Client.Extensions.Msal;

namespace test_msal_extensions2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var cacheDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ".IdentityService");
            StorageCreationProperties storageProperties = new StorageCreationPropertiesBuilder("msal.cache", cacheDirectory)
                    .WithLinuxUnprotectedFile()
                    .Build();

            MsalCacheHelper cacheHelper = MsalCacheHelper.CreateAsync(storageProperties).ConfigureAwait(false).GetAwaiter().GetResult();

            try
            {
                cacheHelper.VerifyPersistence();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to verify persistence.");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
