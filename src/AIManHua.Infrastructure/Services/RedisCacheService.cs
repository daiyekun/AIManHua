using StackExchange.Redis;

namespace AIManHua.Infrastructure.Services;

public class RedisCacheService
{
    private readonly IConnectionMultiplexer _redis;

    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public IDatabase GetDatabase(int db = -1) => _redis.GetDatabase(db);
}
