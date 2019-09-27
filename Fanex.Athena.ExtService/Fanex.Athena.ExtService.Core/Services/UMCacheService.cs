using Microsoft.Extensions.Caching.Distributed;
using MsgPack.Serialization;
using ONEbook.UM.Helpers;
using System;

namespace Fanex.Athena.ExtService.Core.Services
{
    public class UMCacheService : IUMCacheService
    {
        private const string Prefix = "UMSession_";
        private readonly IDistributedCache _cache;

        private static readonly SerializationContext _msgPackSerializationContext = new SerializationContext
        {
            DefaultDateTimeConversionMethod = DateTimeConversionMethod.Native
        };

        public UMCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public T Get<T>(string key)
        {
            var data = _cache.Get(Prefix + key);
            if (data is null)
            {
                return default(T);
            }

            var serializer = MessagePackSerializer.Get<T>(_msgPackSerializationContext);
            return serializer.UnpackSingleObject(data);
        }

        public void Set<T>(string key, T value, TimeSpan absoluteExpiration)
        {
            if (value != null)
            {
                var serializer = MessagePackSerializer.Get<T>(_msgPackSerializationContext);
                var data = serializer.PackSingleObject(value);

                _cache.Set(Prefix + key, data, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = absoluteExpiration
                });
            }
        }
    }
}