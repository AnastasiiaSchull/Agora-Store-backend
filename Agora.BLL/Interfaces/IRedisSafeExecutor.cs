using StackExchange.Redis;

namespace Agora.BLL.Interfaces
{
    public interface IRedisSafeExecutor
    {
        IDatabase TryGetDatabase();
        Task ExecuteAsync(Func<IDatabase, Task> operation, string operationName = null);
        Task SafeSetStringAsync(string key, string value, TimeSpan expiry);
        Task<T?> GetOrSetAsync<T>(string key, Func<Task<T>> fallback, TimeSpan? ttl = null);
    }
}
