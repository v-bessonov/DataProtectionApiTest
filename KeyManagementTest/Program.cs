using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;
using KeyManagementTest;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var keysFolder = Path.Combine(Directory.GetCurrentDirectory(), "temp-keys");

//var serviceCollection = new ServiceCollection();


var services = new ServiceCollection();

// Configure Data Protection to use the same key repository
services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keysFolder));

var serviceProvider = services.BuildServiceProvider();

var dataProtectionProvider = serviceProvider.GetRequiredService<IDataProtectionProvider>();

// Encrypt data
/*var protector = dataProtectionProvider.CreateProtector("CustomPurpose");
var encrypted = protector.Protect("Sensitive Data");

Console.WriteLine($"Encrypted: {encrypted}");*/

// Decrypt data
var protectorForDecryption = dataProtectionProvider.CreateProtector("CustomPurpose");
/*var decrypted = protectorForDecryption.Unprotect(encrypted);

Console.WriteLine($"Decrypted: {decrypted}");*/

var decrypted1 = protectorForDecryption.Unprotect("CfDJ8P0E-gzy39lMv8A3wGjgkuGZtcpp4ubxp2lIEaQ2C1WjU80UcGS39r1zcWVcYcP_Vvj6NeH_XBOCI5KqB6OnMbu1X_fgkDltea2CtE1daBZIDMutkiRCyGe_H0CigxFZsQ");
Console.WriteLine($"Decrypted1: {decrypted1}");

// get a reference to the key manager
var keyManager = serviceProvider.GetService<IKeyManager>();
var allKeys = keyManager.GetAllKeys();

foreach (var key in allKeys)
{
    var en = key.CreateEncryptor().Encrypt(Encoding.UTF8.GetBytes("Sensitive Data"), Encoding.UTF8.GetBytes(string.Empty));
    Console.WriteLine("Serialized Key XML:");
    Console.WriteLine(Encoding.UTF8.GetString(en));
    var ff2 = key.CreateEncryptor().Decrypt(en, Encoding.UTF8.GetBytes(string.Empty));
    Console.WriteLine($"Decrypted Key XML: {Encoding.UTF8.GetString(ff2)}");
}

////var options = serviceProvider.GetService<IOptions<KeyManagementOptions>>();

////var ff = 1;





/*var builder = serviceCollection
    .AddDataProtection()
    .SetApplicationName("KeyManagementTest")
    // point at a specific folder and use DPAPI to encrypt keys
    .PersistKeysToFileSystem(new DirectoryInfo(keysFolder));

/*serviceCollection.AddOptions<KeyManagementOptions>()
    .Configure<IConfiguration>((options, configuration) =>
    {
        configuration.GetSection("KeyManagement").Bind(options);
    });#1#



/*serviceCollection.AddOptions<KeyManagementOptions>()
    .Configure<IServiceScopeFactory>((options, factory) =>
    {
        options.XmlRepository = new CustomXmlRepository(factory);
    });#1#



if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    builder.ProtectKeysWithDpapi();
}


using var services = serviceCollection.BuildServiceProvider();


// perform a protect operation to force the system to put at least
// one key in the key ring
////services.GetDataProtector("Sample.KeyManager.v1").Protect("payload");
////services.GetDataProtector("Sample.KeyManager.v1").Protect("important data");

services.GetDataProtector("Sample.KeyManager.v1").Unprotect("AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAmWdjQiZrlEOuukVtvD/urwAAAAACAAAAAAAQZgAAAAEAACAAAAAhVAeySgZmtSrr9+Zj4vcmTPrKQrcE+W7hhBlyf47vEwAAAAAOgAAAAAIAACAAAAA1ceh8J42Xa3jBcF5yHdEGOGvFBSD4g0mSjCalqXrjKFABAAAmjjbHkpaubwFVAITlyNwq1+tCvvdN4Gbowgrh0Wp1ZTq3SypoAVDUY6KFbC1UzTb/sQV3lfuGI8lNXLpdJBKNT0aWCSHOIGEqXPV4upaR3IFnSnr50EGYc8nRq9FPUVBDXdg/yNVAYnHbjw3MpUwfyITXfj2G9bxKwlJQdDFVYK38/ruDpPfS6nph8PKT1JPKEu+GK77V0gKd7og7AUjdlrItREFxu3bTW155E3lepfxzX5Kkv7aP6hidGNBKusKAEe5BTVwUlRunrmSCEYVLJe7F5znvdSiN6nvouAid/EtgVv2RcgdK6P1t3r6lP85K1k8/n8VjRtiIPC95DdCtzHvNG0Gp4FAVdl562BgxkaDgvwC/7nzGbCK/0SyXCRxoomOAH9qG2KJdJ/MW/WxAmistBwN3clrJATviz8TBYmSxbuEorz8a9WS2mx7hWTBAAAAALfNgzgJo7EmDo1/ZwI/QilQj0AIggJ3ifVIzhaTbMxXp4fhJFlKFysy0JGSk/qPeoxIhzk8bBCPSPuAD1GGv4A==");



Console.WriteLine("Performed a protect operation.");



var keyFiles = Directory.GetFiles(keysFolder, "*.xml");

foreach (var keyFile in keyFiles)
{
    if (keyFile.Contains("key-0c095ae8-d311-4c57-ac1d-4b33221314e1"))
    {
        string content = File.ReadAllText(keyFile);
        Console.WriteLine($"Key File: {keyFile}");
        Console.WriteLine(content);
        var tt = XElement.Parse(content);
         }
    
}


var xmlRepository = services.GetRequiredService<IXmlRepository>();

// Retrieve all persisted key elements
var allKeys1 = xmlRepository.GetAllElements();

foreach (var key in allKeys1)
{
    Console.WriteLine("Serialized Key XML:");
    Console.WriteLine(key);
}


// get a reference to the key manager
var keyManager = services.GetService<IKeyManager>();

// list all keys in the key ring
var allKeys = keyManager.GetAllKeys();
Console.WriteLine($"The key ring contains {allKeys.Count} key(s).");

foreach (var key in allKeys)
{
    Console.WriteLine($"Key {key.KeyId:B}: Created = {key.CreationDate:u}, IsRevoked = {key.IsRevoked}");
}

// revoke all keys in the key ring
keyManager.RevokeAllKeys(DateTimeOffset.Now, reason: "Revocation reason here.");
Console.WriteLine("Revoked all existing keys.");

// add a new key to the key ring with immediate activation and a 1-month expiration
keyManager.CreateNewKey(
    activationDate: DateTimeOffset.Now,
    expirationDate: DateTimeOffset.Now.AddMonths(1));
Console.WriteLine("Added a new key.");

// list all keys in the key ring
allKeys = keyManager.GetAllKeys();
Console.WriteLine($"The key ring contains {allKeys.Count} key(s).");
foreach (var key in allKeys)
{
    Console.WriteLine($"Key {key.KeyId:B}: Created = {key.CreationDate:u}, IsRevoked = {key.IsRevoked}");
}*/