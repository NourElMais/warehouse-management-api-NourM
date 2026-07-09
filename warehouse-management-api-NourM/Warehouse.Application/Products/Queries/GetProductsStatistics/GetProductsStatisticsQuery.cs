using MediatR;
using Warehouse.Application.Products.Queries;

namespace Warehouse.Application.Products.GetProductsStatistics;

public class GetProductsStatisticsQuery : IRequest<GetProductsStatisticsResponse>
{
}