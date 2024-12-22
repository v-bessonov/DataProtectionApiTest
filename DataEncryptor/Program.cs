using System.Runtime.InteropServices;
using System.Text.Json;
using DataEncryptor;
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
var encrypted = protector.Protect("Sensitive Data");

var settings = new Settings(encrypted);
var json = JsonSerializer.Serialize(settings);

var file = new FileInfo(filePath);
file.Directory?.Create();

await File.WriteAllTextAsync(filePath, json);
Console.WriteLine($"Protected data {json} written to: {filePath}");

    
    