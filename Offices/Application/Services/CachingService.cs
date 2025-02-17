using Microsoft.Extensions.Caching.Hybrid;
using OfficeRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CachingService : ICachingService
    {
        private readonly HybridCache _cache;

        public CachingService(HybridCache cache)
        {
            _cache = cache;
        }

        public ValueTask<T> GetOrCreateAsync<T>(string key, Func<CancellationToken, ValueTask<T>> factory, HybridCacheEntryOptions? options = null, IEnumerable<string>? tags = null, CancellationToken token = default)
        {
            return _cache.GetOrCreateAsync(key, factory);
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }

        public async Task SetAsync<T>(string key, T value)
        {
            await _cache.SetAsync(key, value);
        }
    }
}
