using MediatR;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Products;

namespace Warehouse.Application.Products.Queries;

public class GetLowStockProductsQuery : IRequest<List<ProductViewModel>>
{
    public int Threshold { get; set; }

    public GetLowStockProductsQuery(int threshold)
    {
        Threshold = threshold;
    }
}