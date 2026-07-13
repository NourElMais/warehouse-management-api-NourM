using Warehouse.Domain.Repositories;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Infrastructure.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly WarehouseDbContext _db;

    public SupplierRepository(WarehouseDbContext context)
    {
        _db = context;
    }
    public List<Supplier> GetAll()
    {
        return _db.Suppliers.ToList();
    }

    public Supplier? GetById(string id)
    {
        foreach (Supplier supplier in _db.Suppliers.ToList())
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
       _db.Suppliers.Add(supplier);
       _db.SaveChanges();
    }

    public void Update(Supplier supplier)
    {
        _db.SaveChanges();
    }
}