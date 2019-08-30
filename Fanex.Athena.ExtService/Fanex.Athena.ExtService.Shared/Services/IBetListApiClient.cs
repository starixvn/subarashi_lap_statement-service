using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.ExtService.Shared.Services
{
    public interface IBetListApiClient
    {
        Task<T> PostAsync<T>(string path, object value, string language, int timeout = 0);
        Task<string> PostAsyncString(string path, object value, string language, int timeout = 0);

    }
}
