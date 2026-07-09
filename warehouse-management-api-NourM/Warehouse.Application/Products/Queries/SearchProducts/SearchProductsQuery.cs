using MediatR;
using Warehouse.Domain.Products;

namespace Warehouse.Application.Products.Queries;

public class SearchProductsQuery : IRequest<List<Product>>
{
    public string? Name { get; set; }
    public string? Supplier { get; set; }

    public SearchProductsQuery(string? name, string? supplier)
    {
        Name = name;
        Supplier = supplier;
    }
}