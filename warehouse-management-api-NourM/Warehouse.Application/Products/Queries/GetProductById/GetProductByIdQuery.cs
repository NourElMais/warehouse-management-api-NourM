using MediatR;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Products;

namespace Warehouse.Application.Products.Queries;

public class GetProductByIdQuery : IRequest<ProductViewModel?>
{
    public string Id { get; set; }

    public GetProductByIdQuery(string id)
    {
        Id = id;
    }
}