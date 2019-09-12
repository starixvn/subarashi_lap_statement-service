using System.IO;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serializers;

namespace Fanex.Athena.ExtService.Common.Extensions
{
    public class RestSharpNewtonsoftJsonSerializer : ISerializer, IDeserializer
    {
        private readonly Newtonsoft.Json.JsonSerializer serializer;

        public RestSharpNewtonsoftJsonSerializer(Newtonsoft.Json.JsonSerializer serializer)
        {
            this.serializer = serializer;
        }

        public string ContentType
        {
            get { return "application/json"; }
            set { }
        }
        public DateTimeZoneHandling TimeZoneHandling { get; set; } = DateTimeZoneHandling.Unspecified;
        public string DateFormat { get; set; }

        public string Namespace { get; set; }

        public string RootElement { get; set; }

        public string Serialize(object obj)
        {
            using (var stringWriter = new StringWriter())
            {
                using (var jsonTextWriter = new JsonTextWriter(stringWriter))
                {
                    serializer.DateTimeZoneHandling = TimeZoneHandling;
                    serializer.Serialize(jsonTextWriter, obj);

                    return stringWriter.ToString();
                }
            }
        }

        public T Deserialize<T>(RestSharp.IRestResponse response)
        {
            var content = response.Content;

            using (var stringReader = new StringReader(content))
            {
                using (var jsonTextReader = new JsonTextReader(stringReader))
                {
                    return serializer.Deserialize<T>(jsonTextReader);
                }
            }
        }

        public static RestSharpNewtonsoftJsonSerializer Default
        {
            get
            {
                return new RestSharpNewtonsoftJsonSerializer(new Newtonsoft.Json.JsonSerializer
                {
                    NullValueHandling = NullValueHandling.Ignore,
                });
            }
        }
    }
}
