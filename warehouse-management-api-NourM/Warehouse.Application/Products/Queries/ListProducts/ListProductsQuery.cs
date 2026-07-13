using MediatR;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Products;

namespace Warehouse.Application.Products.Queries;

public class ListProductsQuery : IRequest<List<ProductViewModel>>
{
    public bool OnlyAvailable { get; set; }

    public ListProductsQuery(bool onlyAvailable)
    {
        OnlyAvailable = onlyAvailable;
    }
}