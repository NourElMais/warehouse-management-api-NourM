using MediatR;
using Warehouse.Application.ViewModels;

namespace Warehouse.Application.Products.Queries;

public class GetOutOfStockProductsQuery : IRequest<List<ProductViewModel>>
{
}