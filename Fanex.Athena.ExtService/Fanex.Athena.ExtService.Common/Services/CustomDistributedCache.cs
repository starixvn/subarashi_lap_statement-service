using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fanex.Athena.ExtService.Common.Services
{
    public class CustomDistributedCache : ICustomDistributedCache
    {
        private readonly IDistributedCache _sourceCache;
        private readonly ILogger<CustomDistributedCache> _logger;

        public CustomDistributedCache(IDistributedCache sourceCache, ILogger<CustomDistributedCache> logger)
        {
            _sourceCache = sourceCache;
            _logger = logger;
        }

        protected async Task<T> HandleAsync<T>(Func<Task<T>> action)
        {
            try
            {
                var result = await action?.Invoke();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            return default(T);
        }

        protected async Task HandleAsync(Func<Task> action)
        {
            try
            {
                await action?.Invoke();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }

        public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken))
        {
            return HandleAsync(() => _sourceCache.SetAsync(key, value, options, token));
        }

        public Task<byte[]> GetAsync(string key, CancellationToken token = default(CancellationToken))
        {
            return HandleAsync(() => _sourceCache.GetAsync(key, token));
        }

        public Task RemoveAsync(string key, CancellationToken token = default(CancellationToken))
        {
            return HandleAsync(() => _sourceCache.RemoveAsync(key, token));
        }

        public Task RefreshAsync(string key, CancellationToken token = default(CancellationToken))
        {
            return HandleAsync(() => _sourceCache.RefreshAsync(key, token));
        }

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            throw new System.NotImplementedException();
        }

        public byte[] Get(string key)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(string key)
        {
            throw new System.NotImplementedException();
        }

        public void Refresh(string key)
        {
            throw new System.NotImplementedException();
        }
    }
}