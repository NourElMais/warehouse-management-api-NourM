using MediatR;
using Warehouse.Application.Cache.CacheStatistics;
using Warehouse.Application.Interfaces;

namespace Warehouse.Application.Cache.GetCacheStatistics;

public class GetCacheStatisticsHandler:IRequestHandler<GetCacheStatisticsQuery,CacheStatisticsResponse>
{
    private readonly ICacheStatisticsService  _cacheStatistics;

    public GetCacheStatisticsHandler(ICacheStatisticsService cacheStatistics)
    {
        _cacheStatistics = cacheStatistics;
    }
    public Task<CacheStatisticsResponse> Handle(GetCacheStatisticsQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_cacheStatistics.GetStatistics());
    }
}