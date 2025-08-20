namespace Fanex.Athena.ExtService.Common.Services
{
    using System.Threading.Tasks;

    public interface IBetListApiClient
    {
        Task<T> PostAsync<T>(string path, object value, string language);

        Task<string> PostAsyncString(string path, object value, string language);
    }
}