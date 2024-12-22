using System.Xml.Linq;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace KeyManagementTest;

public class CustomXmlRepository : IXmlRepository
{
    private readonly IServiceScopeFactory factory;

    public CustomXmlRepository(IServiceScopeFactory factory)
    {
        this.factory = factory;
    }

    public IReadOnlyCollection<XElement> GetAllElements()
    {
        var keysFolder = Path.Combine(Directory.GetCurrentDirectory(), "temp-keys");
        var keyFiles = Directory.GetFiles(keysFolder, "*.xml");

        List<string> keysAll = new();
        foreach (var keyFile in keyFiles)
        {
            if (keyFile.Contains("key-0c095ae8-d311-4c57-ac1d-4b33221314e1"))
            {
                string content = File.ReadAllText(keyFile);
                keysAll.Add(content);
            }
            
            /*Console.WriteLine($"Key File: {keyFile}");
            Console.WriteLine(content);*/
        }
        var keys = keysAll.ToList()
            .Select(x => XElement.Parse(x))
            .ToList();
        return keys;
        /*using (var scope = factory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<DataProtectionDbContext>();
            var keys = context.XmlKeys.ToList()
                .Select(x => XElement.Parse(x.Xml))
                .ToList();
            return keys;
        }*/
    }

    public void StoreElement(XElement element, string friendlyName)
    {
        /*var key = new XmlKey
        {
            Xml = element.ToString(SaveOptions.DisableFormatting)
        };

        using (var scope = factory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<DataProtectionDbContext>();
            context.XmlKeys.Add(key);
            context.SaveChanges();
        }*/
    }
}