using Warehouse.Application.Cache.CacheStatistics;

namespace Warehouse.Application.Interfaces;

public interface ICacheStatisticsService
{
    //this service must be able to:
    void RecordHit(string key);
    void RecordMiss();
    void RecordRefresh(string key);
    void RemoveKey(string key);

    CacheStatisticsResponse GetStatistics();
}