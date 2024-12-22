using System.Runtime.InteropServices;
using System.Text.Json;
using DataDecriptor;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;

var keysFolder = Path.Combine(Directory.GetCurrentDirectory(), "temp-keys");
var storageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "protected-data");
var filename = "mysecret.dat"; // File to store protected data.
var filePath = Path.Combine(storageDirectory, filename);


var serviceCollection = new ServiceCollection();
var builder = serviceCollection
    .AddDataProtection()
    .SetApplicationName("DataProtectionTest")
    // point at a specific folder and use DPAPI to encrypt keys
    .PersistKeysToFileSystem(new DirectoryInfo(keysFolder));

if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    builder.ProtectKeysWithDpapi();
}
    

var serviceProvider = serviceCollection.BuildServiceProvider();

var dataProtectionProvider = serviceProvider.GetRequiredService<IDataProtectionProvider>();


var protector = dataProtectionProvider.CreateProtector("CustomPurpose");

if (File.Exists(filePath))
{
    string retrievedProtectedData;
    try
    {
        retrievedProtectedData = await File.ReadAllTextAsync(filePath);

    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error reading from file: {ex.Message}");
        return;
    }

    var settings = JsonSerializer.Deserialize<Settings>(retrievedProtectedData);
    
    var unprotectedData = protector.Unprotect(settings?.SensitiveData ?? string.Empty);
    Console.WriteLine($"Unprotected data: {unprotectedData}");

}