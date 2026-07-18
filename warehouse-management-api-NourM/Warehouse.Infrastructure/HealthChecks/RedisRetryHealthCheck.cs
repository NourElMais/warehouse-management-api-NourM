using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;

namespace Warehouse.Infrastructure.HealthChecks;

//IHealthCheck is the interface ASP.NET Core uses for custom health checks.

public class RedisRetryHealthCheck : IHealthCheck
{
    //IConnectionMultiplexer represents the Redis connection -> The health check will use it to test whether Redis responds
    private readonly IConnectionMultiplexer _redis;
    
    public RedisRetryHealthCheck(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }
    
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        for (int attempt = 1; attempt <= 3; attempt++)
        {
            try
            {
                await _redis.GetDatabase().PingAsync(); //this sends a small ping to Redis
                return HealthCheckResult.Healthy("Redis is reachable.");
            }
            catch
            {
                await Task.Delay(500, cancellationToken); //we wait half a second before trying again
            }
        }

        return HealthCheckResult.Unhealthy("Redis is unavailable after 3 attempts.");
    }
}