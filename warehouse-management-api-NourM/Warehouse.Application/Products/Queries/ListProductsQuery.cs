using MediatR;
using Warehouse.Domain.Products;

namespace Warehouse.Application.Products.Queries;

public class ListProductsQuery : IRequest<List<Product>>
{
    public bool OnlyAvailable { get; set; }

    public ListProductsQuery(bool onlyAvailable)
    {
        OnlyAvailable = onlyAvailable;
    }
}