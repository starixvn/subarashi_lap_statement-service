using Fanex.Athena.ExtService.Common.Models.DTO;
using Fanex.Athena.ExtService.Common.Services;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.ExtService.Common.UnitTests.Services
{
    public class BetListApiClientTest
    {
        private readonly Mock<IHttpClientFactory> _mockClientFactory;
        private readonly Mock<HttpClient> _mockClient;
        private readonly Mock<IOptions<BetListApiClientName>> _mockOptions;

        public BetListApiClientTest()
        {
            _mockClientFactory = new Mock<IHttpClientFactory>();
            _mockClient = new Mock<HttpClient> { CallBase = false };
            _mockOptions = new Mock<IOptions<BetListApiClientName>>();
            _mockOptions.SetReturnsDefault(new BetListApiClientName());
            _mockClientFactory.SetReturnsDefault(_mockClient.Object);
            _mockClient.Object.BaseAddress = new Uri("http://test.com");
        }

        [Fact]
        public async Task HandleAsync_WhenCallSuccess_ReturnResult()
        {
            //Arrange
            const string language = "en-US";
            const string FakeContent = "ANY";
            const int timeout = 2;

            _mockClient.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), CancellationToken.None))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(FakeContent) });

            var apiClient = new BetListApiClient(_mockOptions.Object, _mockClientFactory.Object);

            //Action
            var result = await apiClient.PostAsyncString("/test", "test-data", language, timeout).ConfigureAwait(false);

            //Assert

            Assert.Equal(FakeContent, result);
            Assert.Equal(TimeSpan.FromMinutes(timeout), _mockClient.Object.Timeout);
        }

        [Fact]
        public async Task HandleAsync_RequestMessage_ConfigCorrectly()
        {
            //Arrange
            const string ApiClientKey = "test-bet-list-api-client";

            HttpRequestMessage request = null;
            _mockClientFactory.Reset();
            _mockOptions.Object.Value.Name = ApiClientKey;
            _mockClientFactory.Setup(r => r.CreateClient(ApiClientKey)).Returns(_mockClient.Object);
            _mockClient.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), CancellationToken.None))
                .Callback<HttpRequestMessage, CancellationToken>((x, _) => { request = x; })
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("any") });
            _mockClient.Object.BaseAddress = new Uri("http://test/base-address");

            var apiClient = new BetListApiClient(_mockOptions.Object, _mockClientFactory.Object);

            //Action
            var result = await apiClient.PostAsyncString("/test-path", new { data = "test" }, "en-US", 3).ConfigureAwait(false);

            //Assert
            Assert.NotNull(request);
            Assert.Equal(HttpMethod.Post, request.Method);
            Assert.Equal("{\"data\":\"test\"}", await request.Content.ReadAsStringAsync());
            Assert.Equal(new Uri("http://test/base-address/test-path"), request.RequestUri);
        }

        [Fact]
        public async Task HandleAsync_WhenRequestNotOk_ThrowInvalidOperationException()
        {
            //Arrange
            _mockClient.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), CancellationToken.None))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "test-error" });

            var apiClient = new BetListApiClient(_mockOptions.Object, _mockClientFactory.Object);

            //Action
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await apiClient.PostAsyncString("/test", new { data = "test" }, "en-US", 1).ConfigureAwait(false));

            //Assert
            Assert.Equal("Http Error = 400-test-error\n Error occur when query betlist data", ex.Message);
        }

        [Fact]
        public async Task PostAsync_WhenCall_ReturnCorrectDataTypeAsync()
        {
            //Arrange
            const string language = "en-US";
            const int timeout = 2;

            _mockClient.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), CancellationToken.None))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{\"ID\":1,\"Name\":\"alex\"}")
                });

            var apiClient = new BetListApiClient(_mockOptions.Object, _mockClientFactory.Object);

            //Action
            var result = await apiClient.PostAsync<dynamic>("/test", new { data = "test" }, language, timeout).ConfigureAwait(false);

            //Assert
            Assert.IsAssignableFrom<JObject>(result);
            Assert.Equal(
                JsonConvert.SerializeObject(new { ID = 1, Name = "alex" }),
                JsonConvert.SerializeObject(result));
        }
    }
}