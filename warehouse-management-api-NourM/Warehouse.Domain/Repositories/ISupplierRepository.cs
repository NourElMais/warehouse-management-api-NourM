namespace Warehouse.Domain.Repositories;

public class ISupplierRepository
{
    List<Supplier> GetAll();
    Supplier? GetById(string id);
    void Add(Supplier supplier);
    void Update(Supplier supplier);
}