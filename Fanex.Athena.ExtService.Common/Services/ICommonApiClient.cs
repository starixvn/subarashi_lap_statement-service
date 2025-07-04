using System.Threading.Tasks;

namespace Fanex.Athena.ExtService.Common.Services
{
    public interface ICommonApiClient
    {
        Task<T> GetAsync<T>(string path, string languageCode, object value = null, int timeOut = 100);
    }
}