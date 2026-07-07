using Warehouse.Domain.Repositories;
using Warehouse.Domain.Suppliers;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Infrastructure.Repositories;

public class SupplierRepository : ISupplierRepository
{
    public List<Supplier> GetAll()
    {
        return FakeSupplierStore.Suppliers;
    }

    public Supplier? GetById(string id)
    {
        foreach (Supplier supplier in FakeSupplierStore.Suppliers)
        {
            if (supplier.Id == id)
            {
                return supplier;
            }
        }

        return null;
    }

    public void Add(Supplier supplier)
    {
        FakeSupplierStore.Suppliers.Add(supplier);
    }

    public void Update(Supplier supplier)
    {
    }
}