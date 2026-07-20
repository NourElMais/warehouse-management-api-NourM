using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Warehouse.Infrastructure.HealthChecks;

//IHealthCheck is the interface ASP.NET Core uses for custom health checks.

public class RedisRetryHealthCheck : IHealthCheck
{
    //IConnectionMultiplexer represents the Redis connection -> The health check will use it to test whether Redis responds
    private readonly IConnectionMultiplexer _redis;
    private readonly ILogger<RedisRetryHealthCheck> _logger;
    
    public RedisRetryHealthCheck(IConnectionMultiplexer redis, ILogger<RedisRetryHealthCheck> logger)
    {
        _redis = redis;
        _logger = logger;
    }
    
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
    {
        for (int attempt = 1; attempt <= 3; attempt++)
        {
            try
            {
                await _redis.GetDatabase().PingAsync(); //this sends a small ping to Redis
                return HealthCheckResult.Healthy("Redis is reachable.");
            }
            catch(RedisConnectionException ex)  
            {
                _logger.LogWarning(ex, "Redis connection failed on attempt {Attempt}", attempt);
                if (attempt != 3)
                    await Task.Delay(500, cancellationToken); //we wait half a second before trying again
            }
        }

        return HealthCheckResult.Unhealthy("Redis is unavailable after 3 attempts.");
    }
}