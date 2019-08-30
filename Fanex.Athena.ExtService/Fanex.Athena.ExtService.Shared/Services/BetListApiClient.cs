using System;
using System.Threading.Tasks;

using Fanex.Athena.ExtService.Shared.Model.DTO;

using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using RestSharp;

namespace Fanex.Athena.ExtService.Shared.Services
{
    public class BetListApiClient : IBetListApiClient
    {
        private const string KEYNAME = "BETLIST_TOKEN_KEY";

        private const string LANGUAGEKEY = "Language";

        private readonly AppSettings _appSetting;
        public BetListApiClient(IOptions<AppSettings> appSettingsAccessor)
        {
            _appSetting = appSettingsAccessor.Value;
        }
        private async Task<T> HandleAsync<T>(string path, object value,
           string language, Func<IRestResponse, Task<T>> codeBlock, int timeout = 0)
        {
            const int oneMinInMillisecond = 60000;
            var client = new RestClient
            {
                BaseUrl = new Uri(_appSetting.BetListApiHostUrl)
            };

            var request = new RestRequest(path);
            request.AddHeader(KEYNAME, _appSetting.BetListApiTokenValue);
            request.AddHeader(LANGUAGEKEY, language);
            request.AddJsonBody(value);
            request.Method = Method.POST;
            if (timeout > 0)
            {
                client.Timeout = oneMinInMillisecond * timeout;
            }
            var result = await client.ExecuteTaskAsync(request).ConfigureAwait(false);

            if (result.ResponseStatus != ResponseStatus.Completed)
            {
                var errorMessage = $"Http Error ={result.ErrorMessage}\n Error occur when query betlist data";
                throw new InvalidOperationException(errorMessage, result.ErrorException);
            }

            return await codeBlock(result).ConfigureAwait(false);

        }

        public async Task<string> PostAsyncString(string path, object value, string language, int timeout = 0)
        {
            return await HandleAsync(path, value, language, async (x) => await Task.FromResult(x.Content).ConfigureAwait(false)
            , timeout).ConfigureAwait(false);
        }
        public async Task<T> PostAsync<T>(string path, object value, string language, int timeout = 0)
        {
            return await HandleAsync<T>(
                path,
                value,
                language,
                  async (x) =>
                  {
                      return await Task.FromResult(JsonConvert.DeserializeObject<T>(x.Content)).ConfigureAwait(false);
                  }, timeout).ConfigureAwait(false);
        }
    }
}
