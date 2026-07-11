using MediatR;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Application.Suppliers.Queries;

public class GetSupplierByIdQuery : IRequest<SupplierViewModel?>
{
    public string Id { get; set; }

    public GetSupplierByIdQuery(string id)
    {
        Id = id;
    }
}