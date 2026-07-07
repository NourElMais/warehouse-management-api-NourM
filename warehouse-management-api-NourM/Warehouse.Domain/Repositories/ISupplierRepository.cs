namespace Warehouse.Domain.Repositories;

public class ISupplierRepository
{
    public List<Supplier> GetAll();
    public Supplier? GetById(string id);
    public void Add(Supplier supplier);
    public void Update(Supplier supplier);
}