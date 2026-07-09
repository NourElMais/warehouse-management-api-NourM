using Warehouse.Domain.Products;

namespace WareHouse.tests.Products;

public class ProductTests
{
    [Test]
    public void UpdatePrice_ShouldThrowException_WhenPriceIsZero()
    {
        Product product = new Product(
            "Laptop",
            "SKU001",
            "Gaming laptop",
            1000,
            5,
            "Dell",
            DateTime.Now.AddYears(1));
        
        Assert.Throws<ArgumentException>(() =>
        {
            product.UpdatePrice(0);
        });
    }

    [Test]
    public void UpdatePrice_ShouldThrowException_WhenPriceIsNegative()
    {
        Product product = new Product(
            "Laptop",
            "SKU001",
            "Gaming laptop",
            1000,
            5,
            "Dell",
            DateTime.UtcNow.AddYears(1));

        Assert.Throws<ArgumentException>(() => { product.UpdatePrice(-100); });
    }
    [Test]
    public void UpdateQuantity_ShouldThrowException_WhenQuantityIsNegative()
    {
        Product product = new Product(
            "Laptop",
            "SKU001",
            "Gaming laptop",
            1000,
            5,
            "Dell",
            DateTime.UtcNow.AddYears(1));

        Assert.Throws<ArgumentException>(() =>
        {
            product.UpdateQuantity(-5);
        });
    }

    [Test]
    public void ArchivedProduct_CannotUpdatePrice()
    {
        Product product = new Product(
            "Laptop",
            "SKU001",
            "Gaming laptop",
            1000,
            5,
            "Dell",
            DateTime.UtcNow.AddYears(1));

        product.Archive();

        Assert.Throws<InvalidOperationException>(() => { product.UpdatePrice(2000); });
    }
    [Test]
    public void InactiveSupplier_CannotBeAssigned()
    {
        Product product = new Product(
            "Laptop",
            "SKU001",
            "Gaming laptop",
            1000,
            5,
            "Dell",
            DateTime.UtcNow.AddYears(1));

        Assert.Throws<InvalidOperationException>(() =>
        {
            product.AssignSupplier("ABC Supplier", false);
        });
    }
}