using DataProtectionApiTest;
using Microsoft.Extensions.DependencyInjection;

// add data protection services
var serviceCollection = new ServiceCollection();
serviceCollection.AddDataProtection();
var services = serviceCollection.BuildServiceProvider();

// create an instance of MyClass using the service provider
var instance = ActivatorUtilities.CreateInstance<SomeClass>(services);
instance.RunSample();