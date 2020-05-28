using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MsgPack.Serialization;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fanex.Athena.ExtService.Common.Services
{
    public class CustomDistributedCache : ICustomDistributedCache
    {
        private static readonly SerializationContext MsgPackSerializationContext = new SerializationContext
        {
            DefaultDateTimeConversionMethod = DateTimeConversionMethod.Native
        };

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
                _logger.LogCritical(ex, ex.Message);
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
                _logger.LogCritical(ex, ex.Message);
            }
        }

        public async Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken))
            where T : class
        {
            if (value is null)
            {
                return;
            }

            var packValue = await PackAsync(value);
            await HandleAsync(() => _sourceCache.SetAsync(key, packValue, options, token));
        }

        public async Task<T> GetAsync<T>(string key, CancellationToken token = default(CancellationToken))
            where T : class
        {
            var packValue = await HandleAsync(() => _sourceCache.GetAsync(key, token));
            if (packValue is null)
            {
                return default(T);
            }

            var value = await UnpackAsync<T>(packValue);
            return value;
        }

        public Task RemoveAsync(string key, CancellationToken token = default(CancellationToken))
        {
            return HandleAsync(() => _sourceCache.RemoveAsync(key, token));
        }

        public Task RefreshAsync(string key, CancellationToken token = default(CancellationToken))
        {
            return HandleAsync(() => _sourceCache.RefreshAsync(key, token));
        }

        public static DistributedCacheEntryOptions WithAbsoluteExpiration(DateTimeOffset time)
           => new DistributedCacheEntryOptions { AbsoluteExpiration = time };

        public static DistributedCacheEntryOptions WithSlidingExpiration(TimeSpan time)
            => new DistributedCacheEntryOptions { SlidingExpiration = time };

        private static Task<T> UnpackAsync<T>(byte[] data)
        {
            var serializer = MessagePackSerializer.Get<T>(MsgPackSerializationContext);
            return serializer.UnpackSingleObjectAsync(data);
        }

        private static Task<byte[]> PackAsync<T>(T value)
        {
            var serializer = MessagePackSerializer.Get<T>(MsgPackSerializationContext);
            return serializer.PackSingleObjectAsync(value);
        }
    }
}