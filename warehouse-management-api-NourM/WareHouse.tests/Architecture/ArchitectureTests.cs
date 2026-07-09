using System.Reflection;
using Warehouse.Domain.Products;

namespace Warehouse.Tests.Architecture;

public class ArchitectureTests
{
    [Test]
    public void DomainShouldNotDependOnInfrastructure()
    {
        // Arrange
        Assembly domainAssembly = typeof(Product).Assembly;

        // Act
        AssemblyName[] referencedAssemblies = domainAssembly.GetReferencedAssemblies();

        bool dependsOnInfrastructure = false;

        foreach (AssemblyName assembly in referencedAssemblies)
        {
            if (assembly.Name == "Warehouse.Infrastructure")
            {
                dependsOnInfrastructure = true;
            }
        }

        // Assert
        Assert.That(dependsOnInfrastructure, Is.False);
    }
}