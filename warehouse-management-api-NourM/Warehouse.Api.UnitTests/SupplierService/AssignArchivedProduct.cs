using FluentAssertions;
using Warehouse.Domain.Exceptions;
using Warehouse.Domain.Products;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Api.UnitTests.SupplierService;

public class AssignArchivedProduct
{
    //Checking if a product is archived is done in the Product class in the Domain layer, by the EnsureNotArchived() function
    [Fact]
    public void AssignSupplier_ProductIsArchived_ShouldThrowException()
    {
        var product = new Product(
            "Ipad",
            "ipad/123",
            "Ipad 10 air",
            900,
            8,
            "old-supplier-id",
            new DateTime(2027, 7, 27)
        );

        var supplier = new Supplier(
            "Nour",
            "Lebanon",
            "nour@mail.com",
            "03-421605",
            "new-supplier-id"
        );

        product.Archive();
        Assert.Throws<BusinessRuleException>(() => product.AssignSupplier(supplier));
    }
}