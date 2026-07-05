using warehouse_management_api_NourM.Models;

namespace warehouse_management_api_NourM;

public class FakeSupplierStore
{
    public static List<Supplier> Suppliers = new List<Supplier>
    {
    // The ids are hard coded GUIDs that I got from this website: https://www.guidgenerator.com/
    new Supplier
    {
        Id = "59c5bef4-4959-4f02-820b-c7eea2da0623",
        Name = "Zahi Nakad",
        Country = "Lebanon",
        ContactEmail = "zahi.nakad@gmail.com",
        PhoneNumber = "+96170988775",
        IsActive = true
    },

    new Supplier
    {
        Id = "addf3c31-d32d-4abd-b4ae-895c61625038",
        Name = "Moussa Khoury",
        Country = "Lebanon",
        ContactEmail = "moussa.khoury@gmail.com",
        PhoneNumber = "+96171435698",
        IsActive = true
    },

    new Supplier
    {
        Id = "5f6ad38e-f054-43d5-b814-9e65a3f8c442",
        Name = "Emilia Nassar",
        Country = "France",
        ContactEmail = "emilia.nassar@orange.fr",
        PhoneNumber = "+33612345678",
        IsActive = true
    },

    new Supplier
    {
        Id = "1917a11d-8d38-48f2-9338-a26c3a784b84",
        Name = "Nour Maiss",
        Country = "United Arab Emirates",
        ContactEmail = "nour.maiss@gmail.com",
        PhoneNumber = "+971501234567",
        IsActive = true
    },

    new Supplier
    {
        Id = "b5c0a5c9-c99e-4dbd-8ee1-73f5ebbbc482",
        Name = "Sara Safadi",
        Country = "Jordan",
        ContactEmail = "sara.safadi@gmail.com",
        PhoneNumber = "+962791234567",
        IsActive = true
    },

    new Supplier
    {
        Id = "6cc296ea-c097-46a4-a2be-d02eae21e800",
        Name = "Andrea Jabbour",
        Country = "Canada",
        ContactEmail = "andrea.jabbour@gmail.com",
        PhoneNumber = "+14165551234",
        IsActive = true
    },

    new Supplier
    {
        Id = "40370bbd-d578-41a0-a285-5f948b534978",
        Name = "Samir Hamad",
        Country = "Germany",
        ContactEmail = "samir.hamad@gmx.de",
        PhoneNumber = "+4915123456789",
        IsActive = true
    },

    new Supplier
    {
        Id = "c0f8715f-b71c-44f5-8519-e789756d49f8",
        Name = "Lea Noun",
        Country = "Cyprus",
        ContactEmail = "lea.noun@gmail.com",
        PhoneNumber = "+35799123456",
        IsActive = true
    },

    new Supplier
    {
        Id = "f7594a8c-4d80-4c48-b191-001446e110b1",
        Name = "Johnny Abou Rjeily",
        Country = "Lebanon",
        ContactEmail = "johnny.abourjeily@gmail.com",
        PhoneNumber = "+96170122334",
        IsActive = true
    },

    new Supplier
    {
        Id = "14f9b10b-92fc-470b-aac5-eeb6a92de7b7",
        Name = "Mia Mrad",
        Country = "Italy",
        ContactEmail = "mia.mrad@gmail.com",
        PhoneNumber = "+393331234567",
        IsActive = true
    }
};
}