using Warehouse.Application.Interfaces;

namespace Warehouse.Application.Cache.CacheStatistics;

public class CacheStatisticsService:ICacheStatisticsService
{
    private readonly HashSet<string> _cachedKeys = new();

    private int _hitCount;

    private int _missCount;

    private DateTime? _lastCacheRefreshTime;
    
    public void RecordHit(string key)
    {
        _hitCount++;
        _cachedKeys.Add(key);
        
    }

    public void RecordMiss()
    {
        _missCount++;
        
    }

    // Called after data is loaded from the database and stored in Redis.
    public void RecordRefresh(string key)
    {
        _cachedKeys.Add(key);
        _lastCacheRefreshTime = DateTime.UtcNow;
    }

    public void RemoveKey(string key)
    {
        _cachedKeys.Remove(key);
    }

    public CacheStatisticsResponse GetStatistics()
    {
        CacheStatisticsResponse stats = new()
        {
            CachedKeys = _cachedKeys,
            HitCount = _hitCount,
            MissCount = _missCount,
            LastCacheRefreshTime = _lastCacheRefreshTime

        };
        return stats;
    }
    
}