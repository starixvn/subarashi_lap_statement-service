using Microsoft.Extensions.Caching.Distributed;
using System.Threading;
using System.Threading.Tasks;

namespace Fanex.Athena.ExtService.Common.Services
{
    public interface ICustomDistributedCache
    {
        Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken))
            where T : class;

        Task<T> GetAsync<T>(string key, CancellationToken token = default)
            where T : class;

        Task RemoveAsync(string key, CancellationToken token = default);

        Task RefreshAsync(string key, CancellationToken token = default);
    }
}