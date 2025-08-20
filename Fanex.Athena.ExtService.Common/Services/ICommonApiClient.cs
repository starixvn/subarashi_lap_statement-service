namespace Fanex.Athena.ExtService.Common.Services
{
    using System.Threading.Tasks;

    public interface ICommonApiClient
    {
        Task<T> GetAsync<T>(string path, string languageCode = "en-US", object value = null, int timeOut = 100);
    }
}