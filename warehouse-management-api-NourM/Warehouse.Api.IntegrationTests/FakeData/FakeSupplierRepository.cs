using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Api.IntegrationTests.FakeData;

public class FakeSupplierRepository:ISupplierRepository
{
    private readonly List<Supplier> _suppliers;

    public FakeSupplierRepository()
    {
        _suppliers=
        [
            new Supplier("Joy","France","joy@mail.com","81-90875472","ba0d85a1-3913-4753-aeea-6504270e3ab1"),
            new Supplier("Samer","Lebanon","samer@mail.com","81-121345","7926aae7-c9d2-4efc-b9d9-052766e667a5")
        ];
    }

    public Task<List<Supplier>> GetAllAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(_suppliers.ToList());
    }

    public Task<Supplier?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var supplier = _suppliers.FirstOrDefault(x => x.Id == id);

        return Task.FromResult(supplier);
    }

    public Task AddAsync(Supplier entity, CancellationToken cancellationToken)
    {
        _suppliers.Add(entity);
        return Task.CompletedTask;
    }


    public Task UpdateAsync(Supplier entity, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}