using Warehouse.Presentation.Contracts;

namespace Warehouse.Api.IntegrationTests.TestUtilities.Builders;

public class SupplierBuilder
{
    public static CreateSupplierRequest Create()
    {
        return new CreateSupplierRequest
        {
            Name = "Rim",
            Country = "Lebanon",
            ContactEmail = $"supplier-{Guid.NewGuid()}@mail.com",
            PhoneNumber = "81-123456"
        };
    }
}