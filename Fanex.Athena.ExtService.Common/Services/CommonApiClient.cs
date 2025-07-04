using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace Fanex.Athena.ExtService.Common.Services
{
    public class ApiResponse<T>
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public T Result { get; set; }
    }

    public class CommonApiClient : ICommonApiClient
    {
        private const string LanguageKey = "Language";
        private const string HttpClientName = "CommonApiClient";
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private static readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            DateTimeZoneHandling = DateTimeZoneHandling.Unspecified,
            NullValueHandling = NullValueHandling.Ignore,
        };

        public CommonApiClient(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _clientFactory = clientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<T> GetAsync<T>(string path, string languageCode = "en-US", object value = null, int timeOut = 100)
        {
            var queryString = value != null ? $"?{BuildQueryString(value)}" : string.Empty;
            var url = $"{path}{queryString}";

            var response = await GetStringAsync(url, languageCode, timeOut).ConfigureAwait(false);

            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(response);

            return apiResponse?.ErrorCode == 0 ? apiResponse.Result : default;
        }

        private static string BuildQueryString(object value)
        {
            var properties = value.GetType().GetProperties();
            var queryParams = new NameValueCollection();

            foreach (var prop in properties)
            {
                var propValue = prop.GetValue(value);
                if (propValue != null)
                {
                    queryParams.Add(prop.Name, propValue.ToString());
                }
            }

            return string.Join("&", Array.ConvertAll(queryParams.AllKeys, key => $"{HttpUtility.UrlEncode(key)}={HttpUtility.UrlEncode(queryParams[key])}"));
        }

        public virtual async Task<string> GetStringAsync(string path, string language, int timeOut = 100)
        {
            var client = _clientFactory.CreateClient(HttpClientName);
            using var cancelToken = new CancellationTokenSource(new TimeSpan(0, 0, timeOut));
            using var request = new HttpRequestMessage(HttpMethod.Get, new Uri(client.BaseAddress + path));

            request.Headers.Add(LanguageKey, language);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancelToken.Token).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            using var streamReader = new StreamReader(stream);
            using var textWriter = new StringWriter();
            const int chunkLength = 4096;
            var buffer = new char[chunkLength];
            int readChunkLength;

            do
            {
                readChunkLength = streamReader.ReadBlock(buffer, 0, chunkLength);
                textWriter.Write(buffer, 0, readChunkLength);
            } while (readChunkLength > 0);

            return textWriter.ToString();
        }
    }
}