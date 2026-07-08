using Warehouse.Application.Products.Commands;
using Warehouse.Application.Products.Queries;

namespace Warehouse.Tests.Application;

public class ProductUseCaseTests
{
    [Test]
    public async Task CreateProduct_ShouldCallRepositoryAdd()
    {
        // Arrange
        var fakeRepository = new FakeProductRepository();

        var handler = new CreateProductHandler(fakeRepository);

        var command = new CreateProductCommand
        {
            Name = "Laptop",
            SKU = "SKU001",
            Description = "Test laptop",
            Price = 1000,
            QuantityInStock = 5,
            SupplierName = "Dell",
            ExpiryDate = DateTime.UtcNow.AddYears(1)
        };

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(fakeRepository.AddWasCalled, Is.True);
    }

    [Test]
    public async Task GetProductById_ShouldReturnNull_WhenProductDoesNotExist()
    {
        // Arrange
        var fakeRepository = new FakeProductRepository();

        var handler = new GetProductByIdHandler(fakeRepository);

        var query = new GetProductByIdQuery("missing-id");

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Null);
    }
}