namespace Fanex.Athena.ExtService.Core.UnitTests.Services
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Services;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Options;
    using Models;
    using Moq;
    using Moq.Protected;
    using Newtonsoft.Json;
    using Xunit;

    public class OAuthServiceTest
    {
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<IOptions<IdentitySetting>> _mockIdentityOptions;
        private readonly Mock<IOptions<TokenManagement>> _mockTokenOptions;
        private readonly Mock<IMemoryCache> _mockMemoryCache;
        private readonly OAuthService _oAuthService;

        public OAuthServiceTest()
        {
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockIdentityOptions = new Mock<IOptions<IdentitySetting>>();
            _mockTokenOptions = new Mock<IOptions<TokenManagement>>();
            _mockMemoryCache = new Mock<IMemoryCache>();

            // Setup memory cache to return false for TryGetValue (cache miss)
            object cacheValue;
            _mockMemoryCache.Setup(x => x.TryGetValue(It.IsAny<object>(), out cacheValue))
                           .Returns(false);

            // Setup memory cache Set method
            var mockCacheEntry = new Mock<ICacheEntry>();
            _mockMemoryCache.Setup(x => x.CreateEntry(It.IsAny<object>()))
                           .Returns(mockCacheEntry.Object);

            _mockIdentityOptions.Setup(x => x.Value).Returns(new IdentitySetting
            {
                Authority = "http://localhost:5000",
                UserInfoPath = "connect/userinfo",
                TokenPath = "connect/token",
                ClientId = "test-client",
                ClientSecret = "test-secret",
                Scope = "test-scope"
            });

            _mockTokenOptions.Setup(x => x.Value).Returns(new TokenManagement
            {
                AccessExpiration = 60
            });

            _oAuthService = new OAuthService(
                _mockHttpClientFactory.Object,
                _mockIdentityOptions.Object,
                _mockTokenOptions.Object,
                _mockMemoryCache.Object);
        }

        [Fact]
        public async Task GetUserInfo_WithValidToken_ReturnsClaims()
        {
            // Arrange
            var token = "valid_token";
            var responseContent = JsonConvert.SerializeObject(new { sub = "123", name = "test user" });

            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
                });

            var httpClient = new HttpClient(mockMessageHandler.Object) { BaseAddress = new Uri("http://localhost") };
            _mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _oAuthService.GetUserInfo(token);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetUserInfo_WithInvalidToken_ReturnsEmptyClaims()
        {
            // Arrange
            var token = "invalid_token";

            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Unauthorized
                });

            var httpClient = new HttpClient(mockMessageHandler.Object) { BaseAddress = new Uri("http://localhost") };
            _mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _oAuthService.GetUserInfo(token);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}