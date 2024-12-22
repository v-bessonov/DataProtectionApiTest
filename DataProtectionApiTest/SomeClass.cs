using Microsoft.AspNetCore.DataProtection;

namespace DataProtectionApiTest;

public class SomeClass(IDataProtectionProvider provider)
{
    private readonly IDataProtector _protector = provider.CreateProtector("Contoso.SomeClass.v1");

    public void RunSample()
    {
        Console.Write("Enter input: ");
        var input = Console.ReadLine();

        // protect the payload
        var protectedPayload = _protector.Protect(input);
        Console.WriteLine($"Protect returned: {protectedPayload}");

        // unprotect the payload
        var unprotectedPayload = _protector.Unprotect(protectedPayload);
        Console.WriteLine($"Unprotect returned: {unprotectedPayload}");
    }
}