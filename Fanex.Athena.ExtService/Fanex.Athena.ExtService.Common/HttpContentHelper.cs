using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.ExtService.Common
{
    public static class HttpContentHelper
    {
        private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DateTimeZoneHandling = DateTimeZoneHandling.Unspecified
        };

        public static HttpContent ToJsonContent(object obj)
        {
            var json = JsonConvert.SerializeObject(obj, Formatting.None, _serializerSettings);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        public static async Task<T> FromJsonContentAsync<T>(HttpContent jsonContent)
        {
            var byteData = await jsonContent.ReadAsByteArrayAsync();
            var jsonData = Encoding.UTF8.GetString(byteData);
            return JsonConvert.DeserializeObject<T>(jsonData, _serializerSettings);
        }
    }
}