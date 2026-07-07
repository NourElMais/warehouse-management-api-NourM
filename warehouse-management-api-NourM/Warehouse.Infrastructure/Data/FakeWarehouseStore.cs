using Warehouse.Domain.Products;

namespace Warehouse.Infrastructure.Data;

public static class FakeWarehouseStore
{
    public static List<Product> Products = new()
    {
        new Product(
            "Lenovo IdeaPad 3",
            "LAP/101/SLV",
            "15 inch silver Lenovo laptop with 128GB SSD.",
            750,
            18,
            "Zahi Nakad",
            DateTime.UtcNow.AddYears(10),
            "a83411cd-469c-4e2a-97f7-df57dd07452e"
        ),

        new Product(
            "Logitech Wireless Mouse",
            "MOU/102/BLK",
            "Wireless optical mouse with USB receiver.",
            18,
            20,
            "Moussa Khoury",
            DateTime.UtcNow.AddYears(5),
            "b93411cd-989c-4e2a-97f7-df57dd09852e"
        ),

        new Product(
            "Mechanical Keyboard",
            "KEY/103/BLU",
            "Blue mechanical keyboard with lighting.",
            45,
            17,
            "Emilia Nassar",
            DateTime.UtcNow.AddYears(10),
            "h76517cd-567d-4e2a-97f7-df57dd07452e"
        ),

        new Product(
            "Canon Document Scanner",
            "SCN/104/BLK",
            "Compact scanner for high-quality document scanning.",
            160,
            11,
            "Nour Maiss",
            DateTime.UtcNow.AddYears(10),
            "ad4cadad-4ef1-4079-98be-de9909e853d4"
        ),

        new Product(
            "HP Inkjet Printer",
            "PRN/105/BLK",
            "Wireless HP printer suitable for home and office use.",
            220,
            9,
            "Sara Safadi",
            DateTime.UtcNow.AddYears(7),
            "d33a3809-6344-48ef-99b5-f80dfdee1cbd"
        ),

        new Product(
            "Dell 24-inch Monitor",
            "MON/106/BLK",
            "24 inch Full HD monitor with HDMI support.",
            200,
            25,
            "Andrea Jabbour",
            DateTime.UtcNow.AddYears(10),
            "750b4f6a-d7be-41b9-9a98-b7da8a83e3f1"
        ),

        new Product(
            "HP OmniBook 15",
            "LAP/107/NVY",
            "15 inch HP laptop with 256GB SSD and 8GB RAM.",
            830,
            17,
            "Samir Hamad",
            DateTime.UtcNow.AddYears(10),
            "a3edd074-49cf-4af7-a547-8a81376213c1"
        ),

        new Product(
            "Wireless Keyboard",
            "KEY/108/WHT",
            "Slim white wireless keyboard with rechargeable battery.",
            55,
            13,
            "Lea Noun",
            DateTime.UtcNow.AddYears(5),
            "acdf5584-cfda-4b00-8873-0c7dc6ea863c"
        ),

        new Product(
            "Bluetooth Mouse",
            "MOU/109/BLU",
            "Compact blue Bluetooth mouse for laptops.",
            22,
            20,
            "Johnny Abou Rjeily",
            DateTime.UtcNow.AddYears(10),
            "1aa180ac-bd48-4f12-851d-28248a86a713"
        ),

        new Product(
            "Samsung 27 inch Monitor",
            "MON/110/BLK",
            "27 inch Full HD monitor.",
            160,
            18,
            "Mia Mrad",
            DateTime.UtcNow.AddYears(10),
            "d94df8aa-755c-4423-a0d6-b25a8f543e4c"
        )
    };
}