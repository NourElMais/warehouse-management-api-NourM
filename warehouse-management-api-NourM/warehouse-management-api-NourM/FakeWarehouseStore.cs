using warehouse_management_api_NourM.Models;

namespace warehouse_management_api_NourM;

public class FakeWarehouseStore
{
    private List<Product> products = new List<Product>()
    {
        new Product
        {
            Id = "a83411cd-469c-4e2a-97f7-df57dd07452e",
            Name = "Lenovo Laptop",
            SKU = "Lap/123/Silver",
            Description = "Silver Lenovo Laptop, 128GB",
            Price = 765,
            QuantityInStock = 12,
            SupplierName = "Zahi Nakad",
            ExpiryDate = DateTime.Now.AddYears(10),
            IsArchived = false,
            CreatedAt = DateTime.Now,
            LastUpdatedAt = DateTime.Now,
        },
        new Product
        {
            Id = "b93411cd-989c-4i2a-97f7-df57dd09852e",
            Name = "LogiTech Mouse",
            SKU = "Mouse/123/Black",
            Description = "Black LogiTech Mouse",
            Price = 15,
            QuantityInStock = 20,
            SupplierName = "Moussa Khoury",
            ExpiryDate = DateTime.Now.AddYears(4),
            IsArchived = false,
            CreatedAt = DateTime.Now,
            LastUpdatedAt = DateTime.Now,
        },
        new Product
        {
            Id = "h76517cd-567d-7y2a-97f7-uy64dd07452e",
            Name = "Keyboard",
            SKU = "Keyb/123/Blue",
            Description = "Blue Keyboard",
            Price = 35,
            QuantityInStock = 17,
            SupplierName = "Emilia Nassar",
            ExpiryDate = DateTime.Now.AddYears(10),
            IsArchived = false,
            CreatedAt = DateTime.Now,
            LastUpdatedAt = DateTime.Now,
        },
        new Product
        {
            Id = "ad4cadad-4ef1-4079-98be-de9909e853d4",
            Name = "Scanner",
            SKU = "Scanner/123/Black",
            Description = "Black Scanner",
            Price = 150,
            QuantityInStock = 11,
            SupplierName = "Nour Maiss",
            ExpiryDate = DateTime.Now.AddYears(10),
            IsArchived = false,
            CreatedAt = DateTime.Now,
            LastUpdatedAt = DateTime.Now,
        },
        new Product
        {
            Id = "d33a3809-6344-48ef-99b5-f80dfdee1cbd",
            Name = "Printer",
            SKU = "Printer/123/Black",
            Description = "Black HP Printer",
            Price = 200,
            QuantityInStock = 9,
            SupplierName = "Sara Safadi",
            ExpiryDate = DateTime.Now.AddYears(7),
            IsArchived = false,
            CreatedAt = DateTime.Now,
            LastUpdatedAt = DateTime.Now,
        },
        new Product
        {
            Id = "750b4f6a-d7be-41b9-9a98-b7da8a83e3f1",
            Name = "Monitor",
            SKU = "Monit/123/Black",
            Description = "Black Monitor",
            Price = 190,
            QuantityInStock = 25,
            SupplierName = "Andrea Jabbour",
            ExpiryDate = DateTime.Now.AddYears(10),
            IsArchived = false,
            CreatedAt = DateTime.Now,
            LastUpdatedAt = DateTime.Now,
        },
        new Product
        {
            Id = "a3edd074-49cf-4af7-a547-8a81376213c1",
            Name = "HP Laptop",
            SKU = "Lap/128/Navy",
            Description = "Navy HP Laptop, 256GB",
            Price = 600,
            QuantityInStock = 17,
            SupplierName = "Samir Hamad",
            ExpiryDate = DateTime.Now.AddYears(10),
            IsArchived = false,
            CreatedAt = DateTime.Now,
            LastUpdatedAt = DateTime.Now,
        },
        new Product
        {
            Id = "acdf5584-cfda-4b00-8873-0c7dc6ea863c",
            Name = "Keyboard",
            SKU = "Keyb/154/White",
            Description = "White Keyboard",
            Price = 40,
            QuantityInStock = 13,
            SupplierName = "Lea Noun",
            ExpiryDate = DateTime.Now.AddYears(5),
            IsArchived = false,
            CreatedAt = DateTime.Now,
            LastUpdatedAt = DateTime.Now,
        },
        new Product
        {
            Id = "1aa180ac-bd48-4f12-851d-28248a86a713",
            Name = "Mouse",
            SKU = "Mouse/194/Blue",
            Description = "Blue Mouse",
            Price = 15,
            QuantityInStock = 20,
            SupplierName = "Johnny Abou Rjeily",
            ExpiryDate = DateTime.Now.AddYears(10),
            IsArchived = false,
            CreatedAt = DateTime.Now,
            LastUpdatedAt = DateTime.Now,
        },
        new Product
        {
            Id = "d94df8aa-755c-4423-a0d6-b25a8f543e4c",
            Name = "Monitor",
            SKU = "Monitor/156/Black",
            Description = "Black Monitor",
            Price = 230,
            QuantityInStock = 18,
            SupplierName = "Mia Mrad",
            ExpiryDate = DateTime.Now.AddYears(10),
            IsArchived = false,
            CreatedAt = DateTime.Now,
            LastUpdatedAt = DateTime.Now,
        }
    };
}