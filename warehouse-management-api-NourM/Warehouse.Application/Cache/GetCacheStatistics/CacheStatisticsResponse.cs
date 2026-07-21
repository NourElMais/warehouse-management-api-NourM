namespace Warehouse.Application.Cache.CacheStatistics;

public class CacheStatisticsResponse
{
    
    public HashSet<string> CachedKeys { get; set; }

    public int HitCount { get; set; }

    public int MissCount { get; set; }

    public DateTime? LastCacheRefreshTime { get; set; }
}