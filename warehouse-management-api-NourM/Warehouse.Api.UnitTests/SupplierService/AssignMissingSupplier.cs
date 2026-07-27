using Warehouse.Domain.Exceptions;
using Warehouse.Domain.Products;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Api.UnitTests.SupplierService;

public class AssignMissingSupplier
{
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
        supplier.Deactivate();
        Assert.Throws<BusinessRuleException>(() => product.AssignSupplier(supplier));
    }
}