using Warehouse.Domain.DomainEvents;

namespace Warehouse.Domain.Products;

public class Product
{
    public string Id { get; private set; }
    public string Name { get; private set; }
    public string SKU { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public int QuantityInStock { get; private set; }
    public string SupplierName { get; private set; }
    public DateTime ExpiryDate { get; private set; }
    public bool IsArchived { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime LastUpdatedAt { get; private set; }
    
    public List<ProductArchivedEvent> DomainEvents { get; private set; } = new();

    public Product(
        string name,
        string sku,
        string description,
        decimal price,
        int quantityInStock,
        string supplierName,
        DateTime expiryDate,
        string? id = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name is required.");

        if (string.IsNullOrWhiteSpace(sku))
            throw new ArgumentException("SKU is required.");

        if (price <= 0)
            throw new ArgumentException("Price must be greater than zero.");

        if (quantityInStock < 0)
            throw new ArgumentException("Quantity cannot be negative.");

        Id = id ?? Guid.NewGuid().ToString();
        Name = name;
        SKU = sku;
        Description = description;
        Price = price;
        QuantityInStock = quantityInStock;
        SupplierName = supplierName;
        ExpiryDate = expiryDate;
        IsArchived = false;
        CreatedAt = DateTime.UtcNow;
        LastUpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePrice(decimal newPrice)
    {
        EnsureNotArchived();

        if (newPrice <= 0)
            throw new ArgumentException("Price must be greater than zero.");

        Price = newPrice;
        LastUpdatedAt = DateTime.UtcNow;
    }

    public void UpdateQuantity(int newQuantity)
    {
        EnsureNotArchived();

        if (newQuantity < 0)
            throw new ArgumentException("Quantity cannot be negative.");

        QuantityInStock = newQuantity;
        LastUpdatedAt = DateTime.UtcNow;
    }

    public void Archive()
    {
        IsArchived = true;
        LastUpdatedAt = DateTime.UtcNow;

        DomainEvents.Add(new ProductArchivedEvent(Id));
    }

    public void AssignSupplier(string supplierName, bool supplierIsActive)
    {
        EnsureNotArchived();

        if (!supplierIsActive)
            throw new InvalidOperationException("Inactive suppliers cannot be assigned to products.");

        SupplierName = supplierName;
        LastUpdatedAt = DateTime.UtcNow;
    }

    private void EnsureNotArchived()
    {
        if (IsArchived)
            throw new InvalidOperationException("Archived products cannot be updated.");
    }
    
    public void Restore()
    {
        //we verify if it is alreaady active
        if (!IsArchived)
            throw new InvalidOperationException("Product is already active.");

        IsArchived = false;
        LastUpdatedAt = DateTime.UtcNow;
    }
}