using MediatR;
using Warehouse.Domain.Products;

namespace Warehouse.Application.Products.Queries;

public class GetProductByIdQuery : IRequest<Product?>
{
    public string Id { get; set; }

    public GetProductByIdQuery(string id)
    {
        Id = id;
    }
}