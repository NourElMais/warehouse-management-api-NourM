using MediatR;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Application.Suppliers.Queries;

public class GetSupplierByIdQuery : IRequest<Supplier?>
{
    public string Id { get; set; }

    public GetSupplierByIdQuery(string id)
    {
        Id = id;
    }
}