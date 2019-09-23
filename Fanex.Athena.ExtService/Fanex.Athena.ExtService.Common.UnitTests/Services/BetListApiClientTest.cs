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
        private readonly Mock<IOptions<AppSettings>> _mockOptions;
        private readonly Mock<HttpClient> _mockClient;

        private const string BaseUrl = "http://localhost:5000";
        private const string TokenValue = "ANYTOKEN";

        public BetListApiClientTest()
        {
            _mockClientFactory = new Mock<IHttpClientFactory>();
            _mockOptions = new Mock<IOptions<AppSettings>>();
            _mockOptions.Setup(x => x.Value).Returns(new AppSettings
            {
                BetListApiHostUrl = BaseUrl,
                BetListApiTokenValue = TokenValue,
            });
            _mockClient = new Mock<HttpClient> { CallBase = false };
            _mockClientFactory.SetReturnsDefault(_mockClient.Object);
        }

        [Fact]
        public async Task HandleAsync_WhenCallSuccess_PostCorrectParamAndReturnResult()
        {
            //Arrange
            const string language = "en-US";
            const string FakeContent = "ANY";
            const int timeout = 2;

            HttpRequestMessage request = null;
            _mockClient.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), CancellationToken.None))
                .Callback<HttpRequestMessage, CancellationToken>((x, _) => { request = x; })
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(FakeContent) });

            var apiClient = new BetListApiClient(_mockOptions.Object, _mockClientFactory.Object);

            //Action
            var result = await apiClient.PostAsyncString("/test", new { data = "test" }, language, timeout).ConfigureAwait(false);

            //Assert
            Assert.NotNull(request);
            Assert.Equal(HttpMethod.Post, request.Method);
            Assert.Equal("{\"data\":\"test\"}", await request.Content.ReadAsStringAsync());
            Assert.Equal(FakeContent, result);
            Assert.Equal(TimeSpan.FromMinutes(timeout), _mockClient.Object.Timeout);
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