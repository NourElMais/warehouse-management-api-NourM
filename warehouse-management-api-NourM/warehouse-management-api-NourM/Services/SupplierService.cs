using warehouse_management_api_NourM.Contracts;
using warehouse_management_api_NourM.Models;

namespace warehouse_management_api_NourM.Services;

public class SupplierService
{
    public List<Supplier> GetAllActiveSuppliers()
    {
        List<Supplier> activeSuppliers = new List<Supplier>();

        foreach (Supplier s in FakeSupplierStore.Suppliers)
        {
            if (s.IsActive)
            {
                activeSuppliers.Add(s);
            }
        }

        return activeSuppliers;
    }

    public Supplier? GetSupplierById(string id)
    {
        foreach (Supplier s in FakeSupplierStore.Suppliers)
        {
            if (s.Id == id)
            {
                return s;
            }
        }

        return null;
    }

    public Supplier CreateSupplier(CreateSupplierRequest supplier)
    {
        Supplier s = new Supplier
        {
            Id = Guid.NewGuid().ToString(),
            Name = supplier.Name,
            Country = supplier.Country,
            ContactEmail = supplier.ContactEmail,
            PhoneNumber = supplier.PhoneNumber,
            IsActive = true
        };

        FakeSupplierStore.Suppliers.Add(s);
        return s;
    }

    public string DeactivateSupplier(string id)
    {
        Supplier? supplier = GetSupplierById(id);

        if (supplier == null)
        {
            return "not found";
        }

        if (!supplier.IsActive)
        {
            return "already deactivated";
        }

        supplier.IsActive = false;
        return "deactivated";
    }

    public string GetSupplierStatistics()
    {
        int totalSuppliers = FakeSupplierStore.Suppliers.Count;
        int activeSuppliers = 0;
        int inactiveSuppliers = 0;

        List<string> countries = new List<string>();

        foreach (Supplier s in FakeSupplierStore.Suppliers)
        {
            if (s.IsActive)
            {
                activeSuppliers++;
            }
            else
            {
                inactiveSuppliers++;
            }

            if (!countries.Contains(s.Country))
            {
                countries.Add(s.Country);
            }
        }

        string countriesList = "";

        for (int i = 0; i < countries.Count; i++)
        {
            countriesList += countries[i];

            if (i != countries.Count - 1)
            {
                countriesList += ", ";
            }
        }

        return $"Total Suppliers: {totalSuppliers}\n" +
               $"Active Suppliers: {activeSuppliers}\n" +
               $"Inactive Suppliers: {inactiveSuppliers}\n" +
               $"Supplier Countries: {countriesList}";
    }
}