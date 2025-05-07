using Agora.BLL.Interfaces;
using StackExchange.Redis;
using System.Text.Json;
using Microsoft.Extensions.Logging;


    public class RedisSafeExecutor : IRedisSafeExecutor
    {
        // connection to Redis:
        private readonly IConnectionMultiplexer _redis;
        private readonly ILogger<RedisSafeExecutor> _logger;

        public RedisSafeExecutor(IConnectionMultiplexer redis, ILogger<RedisSafeExecutor> logger)
        {
            _redis = redis;
            _logger = logger;
        }

        public IDatabase? TryGetDatabase()
        {
            try
            {
                if (_redis == null || !_redis.IsConnected)
                {
                    _logger.LogWarning("Redis is not connected.");
                    return null;
                }

                return _redis.GetDatabase();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error accessing Redis connection.");
                return null;
            }
        }

        // execute Redis operations:
        public async Task ExecuteAsync(Func<IDatabase, Task> operation, string? operationName = null)
        {
            var db = TryGetDatabase();
            if (db == null)
            {
                _logger.LogWarning("Redis unavailable. Operation skipped{Operation}.", operationName != null ? $" ({operationName})" : "");
                return;
            }

            try
            {
                await operation(db);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis operation error{Operation}.", operationName != null ? $" ({operationName})" : "");
            }
        }

        // safely sets a string value by key:
        public async Task SafeSetStringAsync(string key, string value, TimeSpan expiry)
        {
            await ExecuteAsync(async db =>
            {
                await db.StringSetAsync(key, value, expiry);
            }, $"SET {key}");
        }

        // if Redis not connected -> fallback and try again save to cache
        public async Task<T?> GetOrSetAsync<T>(string key, Func<Task<T>> fallback, TimeSpan? ttl = null)
        {
            T? result = default;
            bool fallbackCalled = false;

            await ExecuteAsync(async db =>
            {
                try
                {
                    var value = await db.StringGetAsync(key);
                    if (!value.IsNullOrEmpty)
                    {
                        result = JsonSerializer.Deserialize<T>(value!);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Redis GET error for key: {Key}", key);
                }

                result = await fallback();
                fallbackCalled = true;

                try
                {
                    if (result != null)
                    {
                        var json = JsonSerializer.Serialize(result);
                        await db.StringSetAsync(key, json, ttl ?? TimeSpan.FromMinutes(30));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Redis SET error for key: {Key}", key);
                }
            }, $"GET/SET {key}");

            if (!fallbackCalled && result == null)
            {
                _logger.LogWarning("Redis completely unavailable. Executing fallback for key: {Key}", key);
                result = await fallback();
            }

            return result;
        }
    }



