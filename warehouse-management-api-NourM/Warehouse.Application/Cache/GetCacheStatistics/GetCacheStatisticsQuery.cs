using MediatR;
using Warehouse.Application.Cache.CacheStatistics;

namespace Warehouse.Application.Cache;

public class GetCacheStatisticsQuery:IRequest<CacheStatisticsResponse>
{
    
}