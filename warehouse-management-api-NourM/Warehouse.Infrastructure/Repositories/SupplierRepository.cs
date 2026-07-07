using Warehouse.Domain.Suppliers;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Infrastructure.Repositories;

public class SupplierRepository
{
    public List<Supplier> GetAll()
    {
        return FakeSupplierStore.Suppliers;
    }

    public Supplier? GetById(string id)
    {
        return FakeSupplierStore.Suppliers
            .FirstOrDefault(s => s.Id == id);
    }

    public void Add(Supplier supplier)
    {
        FakeSupplierStore.Suppliers.Add(supplier);
    }

    public void Update(Supplier supplier)
    {
        // In-memory list stores the same object reference,
        // so no extra code is needed for now.
    }
}