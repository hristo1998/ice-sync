using IceSync.Core.SyncService.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace IceSync.Core.SyncService
{
    public class MemoryHashStore : IHashStore
    {
        private readonly IMemoryCache cache;
        public MemoryHashStore(IMemoryCache cache) => this.cache = cache;

        public Task<string?> GetAsync(string key)
            => Task.FromResult(cache.TryGetValue<string>(key, out var v) ? v : null);

        public Task SetAsync(string key, string hash)
        {
            cache.Set(key, hash);
            return Task.CompletedTask;
        }
    }
}
