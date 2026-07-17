namespace Warehouse.Domain.Repositories;

public interface IRepository<T>
{
    Task<List<T>> GetAllAsync(CancellationToken cancellationToken);

    Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken);

    Task AddAsync(T entity, CancellationToken cancellationToken);

    Task UpdateAsync(T entity, CancellationToken cancellationToken);
}