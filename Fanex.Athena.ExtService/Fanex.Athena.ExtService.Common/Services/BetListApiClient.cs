using Fanex.Athena.ExtService.Common.Models.DTO;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fanex.Athena.ExtService.Common.Services
{
    public class BetListApiClient : IBetListApiClient
    {
        private const string LANGUAGEKEY = "Language";

        private readonly string _betListApiClientName;
        private readonly IHttpClientFactory _clientFactory;

        private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DateTimeZoneHandling = DateTimeZoneHandling.Unspecified
        };

        public BetListApiClient(IOptions<BetListApiClientName> options, IHttpClientFactory clientFactory)
        {
            _betListApiClientName = options.Value.Name;
            _clientFactory = clientFactory;
        }

        private async Task<T> HandleAsync<T>(string path, object value,
           string language, Func<HttpContent, Task<T>> codeBlock, int timeout = 0)
        {
            var client = _clientFactory.CreateClient(_betListApiClientName);
            var request = new HttpRequestMessage(HttpMethod.Post, path)
            {
                Content = ToJsonContent(value)
            };
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
            return await HandleAsync(path, value, language, FromJsonContentAsync<T>, timeout).ConfigureAwait(false);
        }

        private static HttpContent ToJsonContent(object obj)
        {
            using (var stringWriter = new StringWriter())
            {
                using (var jsonWriter = new JsonTextWriter(stringWriter))
                {
                    var serializer = JsonSerializer.Create(_serializerSettings);
                    serializer.Serialize(jsonWriter, obj);
                    return new StringContent(stringWriter.ToString(), Encoding.UTF8, "application/json");
                }
            }
        }

        private static async Task<T> FromJsonContentAsync<T>(HttpContent jsonContent)
        {
            var content = await jsonContent.ReadAsStringAsync();
            using (var stringReader = new StringReader(content))
            {
                using (var jsonTextReader = new JsonTextReader(stringReader))
                {
                    var serializer = JsonSerializer.Create(_serializerSettings);
                    return serializer.Deserialize<T>(jsonTextReader);
                }
            }
        }
    }
}