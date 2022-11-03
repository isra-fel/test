using System;
using System.IO;
using Microsoft.Identity.Client.Extensions.Msal;

namespace test_msal_extensions
{
    public class Program
    {
        public static void Main()
        {
            var cacheDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ".IdentityService");
            StorageCreationProperties storageProperties = new StorageCreationPropertiesBuilder("msal.cache", cacheDirectory)
                    .WithLinuxUnprotectedFile()
                    .Build();

            MsalCacheHelper cacheHelper = MsalCacheHelper.CreateAsync(storageProperties).ConfigureAwait(false).GetAwaiter().GetResult();

            try
            {
                cacheHelper.VerifyPersistence();
                Console.WriteLine("Succeeded!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to verify persistence.");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
