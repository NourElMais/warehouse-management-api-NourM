using Warehouse.Presentation.Contracts;

namespace Warehouse.Api.IntegrationTests.TestUtilities.Builders;

public class ProductBuilder
{
    public static CreateProductRequest Create()
    {
        return new CreateProductRequest
        {
            Name = "Ipad",
            SKU = $"SKU-{Guid.NewGuid()}",
            Description = "Ipad Air",
            Price = 900,
            QuantityInStock = 8,
            ExpiryDate = DateTime.UtcNow.AddYears(1)
        };
    }
}