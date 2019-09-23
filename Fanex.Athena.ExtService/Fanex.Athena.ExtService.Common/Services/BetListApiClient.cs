using Fanex.Athena.ExtService.Common.Models.DTO;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Fanex.Athena.ExtService.Common.Services
{
    public class BetListApiClient : IBetListApiClient
    {
        private const string KEYNAME = "BETLIST_TOKEN_KEY";
        private const string LANGUAGEKEY = "Language";

        private readonly AppSettings _appSetting;
        private readonly IHttpClientFactory _clientFactory;

        public BetListApiClient(IOptions<AppSettings> appSettingsAccessor, IHttpClientFactory clientFactory)
        {
            _appSetting = appSettingsAccessor.Value;
            _clientFactory = clientFactory;
        }

        private async Task<T> HandleAsync<T>(string path, object value,
           string language, Func<HttpContent, Task<T>> codeBlock, int timeout = 0)
        {
            var client = _clientFactory.CreateClient();
            var baseUri = new Uri(_appSetting.BetListApiHostUrl);
            var request = new HttpRequestMessage(HttpMethod.Post, new Uri(baseUri, path))
            {
                Content = HttpContentHelper.ToJsonContent(value)
            };
            request.Headers.Add(KEYNAME, _appSetting.BetListApiTokenValue);
            request.Headers.Add(LANGUAGEKEY, language);

            if (timeout > 0)
            {
                client.Timeout = TimeSpan.FromMinutes(timeout);
            }

            var result = await client.SendAsync(request, CancellationToken.None);
            if (!result.IsSuccessStatusCode)
            {
                var errorMessage = $"Http Error = {(int)result.StatusCode}-{result.ReasonPhrase}\n Error occur when query betlist data";
                throw new InvalidOperationException(errorMessage);
            }

            return await codeBlock(result.Content).ConfigureAwait(false);
        }

        public async Task<string> PostAsyncString(string path, object value, string language, int timeout = 0)
        {
            return await HandleAsync(path, value, language, x => x.ReadAsStringAsync(), timeout).ConfigureAwait(false);
        }

        public async Task<T> PostAsync<T>(string path, object value, string language, int timeout = 0)
        {
            return await HandleAsync(path, value, language, HttpContentHelper.FromJsonContentAsync<T>, timeout).ConfigureAwait(false);
        }
    }
}