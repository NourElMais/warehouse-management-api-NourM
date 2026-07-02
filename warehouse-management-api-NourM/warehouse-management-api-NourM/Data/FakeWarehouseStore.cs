using warehouse_management_api_NourM.Models;

namespace warehouse_management_api_NourM;

public class FakeWarehouseStore
{
    public static List<Product> Products = new List<Product>()
    {
       new Product
    {
        Id = "a83411cd-469c-4e2a-97f7-df57dd07452e",
        Name = "Lenovo IdeaPad 3",
        SKU = "LAP/101/SLV",
        Description = "15 inch silver Lenovo laptop with 128GB SSD.",
        Price = 750,
        QuantityInStock = 18,
        SupplierName = "Zahi Nakad",
        ExpiryDate = DateTime.Now.AddYears(10),
        IsArchived = false,
        CreatedAt = DateTime.Now,
        LastUpdatedAt = DateTime.Now,
    },

    new Product
    {
        Id = "b93411cd-989c-4e2a-97f7-df57dd09852e",
        Name = "Logitech Wireless Mouse",
        SKU = "MOU/102/BLK",
        Description = "Wireless optical mouse with USB receiver.",
        Price = 18,
        QuantityInStock = 20,
        SupplierName = "Moussa Khoury",
        ExpiryDate = DateTime.Now.AddYears(5),
        IsArchived = false,
        CreatedAt = DateTime.Now,
        LastUpdatedAt = DateTime.Now,
    },

    new Product
    {
        Id = "h76517cd-567d-4e2a-97f7-df57dd07452e",
        Name = "Mechanical Keyboard",
        SKU = "KEY/103/BLU",
        Description = "Blue mechanical keyboard with lighting.",
        Price = 45,
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
        Name = "Canon Document Scanner",
        SKU = "SCN/104/BLK",
        Description = "Compact scanner for high-quality document scanning.",
        Price = 160,
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
        Name = "HP Inkjet Printer",
        SKU = "PRN/105/BLK",
        Description = "Wireless HP printer suitable for home and office use.",
        Price = 220,
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
        Name = "Dell 24-inch Monitor",
        SKU = "MON/106/BLK",
        Description = "24 inch Full HD monitor with HDMI support.",
        Price = 200,
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
        Name = "HP OmniBook 15",
        SKU = "LAP/107/NVY",
        Description = "15 inch HP laptop with 256GB SSD and 8GB RAM.",
        Price = 830,
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
        Name = "Wireless Keyboard",
        SKU = "KEY/108/WHT",
        Description = "Slim white wireless keyboard with rechargeable battery.",
        Price = 55,
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
        Name = "Bluetooth Mouse",
        SKU = "MOU/109/BLU",
        Description = "Compact blue Bluetooth mouse for laptops.",
        Price = 22,
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
        Name = "Samsung 27 inch Monitor",
        SKU = "MON/110/BLK",
        Description = "27 inch Full HD monitor.",
        Price = 160,
        QuantityInStock = 18,
        SupplierName = "Mia Mrad",
        ExpiryDate = DateTime.Now.AddYears(10),
        IsArchived = false,
        CreatedAt = DateTime.Now,
        LastUpdatedAt = DateTime.Now,
    }
};
}