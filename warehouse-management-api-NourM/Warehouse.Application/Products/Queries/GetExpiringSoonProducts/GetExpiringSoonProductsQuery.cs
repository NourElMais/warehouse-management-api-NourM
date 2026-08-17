using MediatR;
using Warehouse.Application.ViewModels;

namespace Warehouse.Application.Products.Queries;

public class GetExpiringSoonProductsQuery : IRequest<List<ProductViewModel>>
{
    public int DaysAhead { get; }

    public GetExpiringSoonProductsQuery(int daysAhead)
    {
        DaysAhead = daysAhead;
    }
}