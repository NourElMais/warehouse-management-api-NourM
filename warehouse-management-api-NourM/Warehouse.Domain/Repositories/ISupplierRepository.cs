using Warehouse.Domain.Suppliers;

namespace Warehouse.Domain.Repositories;

public interface ISupplierRepository
{
    public List<Supplier> GetAll();
    public Supplier? GetById(string id);
    public void Add(Supplier supplier);
 
}