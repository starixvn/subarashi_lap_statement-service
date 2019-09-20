using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using ONEbook.UM.Configurations;
using ONEbook.UM.Helpers;
using RestSharp;
using RestSharp.Authenticators;

namespace Fanex.Athena.ExtService.Core.Services
{
    public class UMHmacAuthenticator : IAuthenticator
    {
        private readonly UMClientConfiguration configuration;
        private readonly IPHelper ipHelper;

        public UMHmacAuthenticator(UMClientConfiguration configuration, IPHelper ipHelper)
        {
            this.configuration = configuration;
            this.ipHelper = ipHelper;
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client), "Rest Client cannot be null");
            }

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Rest Request cannot be null");
            }

            var machineIP = ipHelper.GetMachineIP();

            var requestUri = HttpUtility.UrlEncode(client.BuildUri(request).OriginalString.ToLowerInvariant());

            var requestHttpMethod = request.Method.ToString();

            // Calculate UNIX time
            var epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            var timeSpan = DateTime.UtcNow - epochStart;
            var requestTimeStamp = Convert.ToUInt64(timeSpan.TotalSeconds).ToString();

            // create random nonce for each request
            var nonce = Guid.NewGuid().ToString("N");

            // Creating the raw signature string
            var signatureRawData = string.Format("{0}{1}{2}{3}{4}", machineIP, requestHttpMethod, requestUri, requestTimeStamp, nonce);

            var secretKeyByteArray = Convert.FromBase64String(configuration.SecretKey);

            var signature = Encoding.UTF8.GetBytes(signatureRawData);

            using (HMACSHA256 hmac = new HMACSHA256(secretKeyByteArray))
            {
                var signatureBytes = hmac.ComputeHash(signature);
                var requestSignatureBase64String = Convert.ToBase64String(signatureBytes);

                // Setting the values in the Authorization header using custom scheme (amx)
                var authHeader = $"amx {$"{machineIP}:{requestSignatureBase64String}:{nonce}:{requestTimeStamp}"}";
                request.AddParameter("Authorization", authHeader, ParameterType.HttpHeader);
            }
        }

    }
}