using Warehouse.Domain.Suppliers;

namespace Warehouse.Infrastructure.Data;

public static class FakeSupplierStore
{
    public static List<Supplier> Suppliers = new()
    {
        new Supplier(
            "Zahi Nakad",
            "Lebanon",
            "zahi.nakad@gmail.com",
            "+96170988775",
            "59c5bef4-4959-4f02-820b-c7eea2da0623"
        ),

        new Supplier(
            "Moussa Khoury",
            "Lebanon",
            "moussa.khoury@gmail.com",
            "+96171435698",
            "addf3c31-d32d-4abd-b4ae-895c61625038"
        ),

        new Supplier(
            "Emilia Nassar",
            "France",
            "emilia.nassar@orange.fr",
            "+33612345678",
            "5f6ad38e-f054-43d5-b814-9e65a3f8c442"
        ),

        new Supplier(
            "Nour Maiss",
            "United Arab Emirates",
            "nour.maiss@gmail.com",
            "+971501234567",
            "1917a11d-8d38-48f2-9338-a26c3a784b84"
        ),

        new Supplier(
            "Sara Safadi",
            "Jordan",
            "sara.safadi@gmail.com",
            "+962791234567",
            "b5c0a5c9-c99e-4dbd-8ee1-73f5ebbbc482"
        ),

        new Supplier(
            "Andrea Jabbour",
            "Canada",
            "andrea.jabbour@gmail.com",
            "+14165551234",
            "6cc296ea-c097-46a4-a2be-d02eae21e800"
        ),

        new Supplier(
            "Samir Hamad",
            "Germany",
            "samir.hamad@gmx.de",
            "+4915123456789",
            "40370bbd-d578-41a0-a285-5f948b534978"
        ),

        new Supplier(
            "Lea Noun",
            "Cyprus",
            "lea.noun@gmail.com",
            "+35799123456",
            "c0f8715f-b71c-44f5-8519-e789756d49f8"
        ),

        new Supplier(
            "Johnny Abou Rjeily",
            "Lebanon",
            "johnny.abourjeily@gmail.com",
            "+96170122334",
            "f7594a8c-4d80-4c48-b191-001446e110b1"
        ),

        new Supplier(
            "Mia Mrad",
            "Italy",
            "mia.mrad@gmail.com",
            "+393331234567",
            "14f9b10b-92fc-470b-aac5-eeb6a92de7b7"
        )
    };
}