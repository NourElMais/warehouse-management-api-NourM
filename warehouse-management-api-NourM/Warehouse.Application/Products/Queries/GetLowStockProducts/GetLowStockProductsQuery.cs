using MediatR;
using Warehouse.Domain.Products;

namespace Warehouse.Application.Products.Queries;

public class GetLowStockProductsQuery : IRequest<List<Product>>
{
    public int Threshold { get; set; }

    public GetLowStockProductsQuery(int threshold)
    {
        Threshold = threshold;
    }
}