using System;
using System.Net;
using System.Threading.Tasks;

using ONEbook.UM.Configurations;
using ONEbook.UM.Helpers;

using RestSharp;
using RestSharp.Authenticators;

namespace Fanex.Athena.ExtService.Core.Services
{
    public class UMRestClient : IUMRestClient
    {
        private readonly IRestClient _restClient;
        private readonly IAuthenticator _authenticator;
        public UMRestClient(UMClientConfiguration configuration,
            IAuthenticator _authenticator)
        {
            _restClient = new RestClient
            {
                BaseUrl = new Uri(configuration.ServiceUri),
                Authenticator = _authenticator,
                Timeout = 10000
            };

        }

        public async Task<T> ExecuteGetAsync<T>(string endPoint)
        {
            return await ExecuteAsync<T>(endPoint, Method.GET, null).ConfigureAwait(false);
        }

        public async Task<T> ExecutePostAsync<T>(string endPoint, object data = null)
        {
            return await ExecuteAsync<T>(endPoint, Method.POST, data).ConfigureAwait(false);
        }
        public T ExecuteGet<T>(string endPoint) where T : new()
        {
            return Execute<T>(endPoint, Method.GET, null);

        }
        public T ExecutePost<T>(string endPoint, object data = null) where T : new()
        {
            return Execute<T>(endPoint, Method.POST, data);
        }

        private async Task<T> ExecuteAsync<T>(string endPoint, Method method, object data)
        {
            ValidateEndPoint(endPoint);

            var request = new RestRequest(endPoint, method);

            if (data != null)
            {
                request.AddJsonBody(data);
            }

            var response = await _restClient.ExecuteTaskAsync<T>(request).ConfigureAwait(false);

            if (response.IsSuccessful)
            {
                return response.Data;
            }

            if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                await Task.FromException(new Exception(response.Content)).ConfigureAwait(false);
            }

            throw new InvalidOperationException(
                string.Join(
                    "\r\n",
                    $"Content: {response.Content}",
                    $"ErrorMessage: {response.ErrorMessage}"));
        }
        private T Execute<T>(string endPoint, Method method, object data) where T : new()
        {
            ValidateEndPoint(endPoint);

            var request = new RestRequest(endPoint, method);

            if (data != null)
            {
                request.AddJsonBody(data);
            }

            var response = _restClient.Execute<T>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }

            if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                throw new Exception(response.Content);
            }

            throw new InvalidOperationException(
                string.Join(
                    "\r\n",
                    $"Content: {response.Content}",
                    $"ErrorMessage: {response.ErrorMessage}"));
        }
        private static void ValidateEndPoint(string endPoint)
        {
            if (string.IsNullOrWhiteSpace(endPoint))
            {
                throw new ArgumentNullException(nameof(endPoint));
            }
        }

    }
}
