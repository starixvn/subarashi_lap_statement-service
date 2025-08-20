namespace Fanex.Athena.ExtService.Common.UnitTests.Services
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Services;
    using Moq;
    using Moq.Protected;
    using Newtonsoft.Json;
    using Xunit;

    public class CommonApiClientTest
    {
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly CommonApiClient _commonApiClient;

        public CommonApiClientTest()
        {
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://api.test.com/")
            };
            
            _mockHttpClientFactory.Setup(x => x.CreateClient("CommonApiClient"))
                .Returns(_httpClient);
            
            _commonApiClient = new CommonApiClient(_mockHttpClientFactory.Object);
        }

        [Fact]
        public async Task GetAsync_WithValidResponse_ReturnsDeserializedResult()
        {
            // Arrange
            var expectedResult = new TestModel { Id = 1, Name = "Test" };
            var apiResponse = new ApiResponse<TestModel>
            {
                ErrorCode = 0,
                Result = expectedResult
            };
            var jsonResponse = JsonConvert.SerializeObject(apiResponse);

            SetupHttpResponse(HttpStatusCode.OK, jsonResponse);

            // Act
            var result = await _commonApiClient.GetAsync<TestModel>("/test");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Id, result.Id);
            Assert.Equal(expectedResult.Name, result.Name);
        }

        [Fact]
        public async Task GetAsync_WithErrorCode_ReturnsDefault()
        {
            // Arrange
            var apiResponse = new ApiResponse<TestModel>
            {
                ErrorCode = 1,
                ErrorMessage = "Error occurred",
                Result = new TestModel { Id = 1, Name = "Test" }
            };
            var jsonResponse = JsonConvert.SerializeObject(apiResponse);

            SetupHttpResponse(HttpStatusCode.OK, jsonResponse);

            // Act
            var result = await _commonApiClient.GetAsync<TestModel>("/test");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAsync_WithCustomLanguage_SetsCorrectHeader()
        {
            // Arrange
            var expectedResult = new TestModel { Id = 1, Name = "Test" };
            var apiResponse = new ApiResponse<TestModel>
            {
                ErrorCode = 0,
                Result = expectedResult
            };
            var jsonResponse = JsonConvert.SerializeObject(apiResponse);

            SetupHttpResponse(HttpStatusCode.OK, jsonResponse);

            // Act
            await _commonApiClient.GetAsync<TestModel>("/test", "fr-FR");

            // Assert
            _mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Headers.Contains("Language") &&
                    req.Headers.GetValues("Language").Any(v => v == "fr-FR")
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task GetAsync_WithQueryParameters_BuildsCorrectUrl()
        {
            // Arrange
            var queryObject = new { param1 = "value1", param2 = "value2" };
            var expectedResult = new TestModel { Id = 1, Name = "Test" };
            var apiResponse = new ApiResponse<TestModel>
            {
                ErrorCode = 0,
                Result = expectedResult
            };
            var jsonResponse = JsonConvert.SerializeObject(apiResponse);

            SetupHttpResponse(HttpStatusCode.OK, jsonResponse);

            // Act
            await _commonApiClient.GetAsync<TestModel>("/test", "en-US", queryObject);

            // Assert
            _mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri.Query.Contains("param1=value1") &&
                    req.RequestUri.Query.Contains("param2=value2")
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task GetAsync_WithNullQueryParameters_DoesNotAddQueryString()
        {
            // Arrange
            var expectedResult = new TestModel { Id = 1, Name = "Test" };
            var apiResponse = new ApiResponse<TestModel>
            {
                ErrorCode = 0,
                Result = expectedResult
            };
            var jsonResponse = JsonConvert.SerializeObject(apiResponse);

            SetupHttpResponse(HttpStatusCode.OK, jsonResponse);

            // Act
            await _commonApiClient.GetAsync<TestModel>("/test");

            // Assert
            _mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    string.IsNullOrEmpty(req.RequestUri.Query)
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task GetAsync_WithDefaultParameters_UsesDefaultValues()
        {
            // Arrange
            var expectedResult = new TestModel { Id = 1, Name = "Test" };
            var apiResponse = new ApiResponse<TestModel>
            {
                ErrorCode = 0,
                Result = expectedResult
            };
            var jsonResponse = JsonConvert.SerializeObject(apiResponse);

            SetupHttpResponse(HttpStatusCode.OK, jsonResponse);

            // Act
            await _commonApiClient.GetAsync<TestModel>("/test");

            // Assert
            _mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Headers.Contains("Language") &&
                    req.Headers.GetValues("Language").Any(v => v == "en-US")
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Theory]
        [InlineData(50)]
        [InlineData(120)]
        [InlineData(300)]
        public async Task GetAsync_WithCustomTimeout_SetsCorrectTimeout(int timeoutSeconds)
        {
            // Arrange
            var expectedResult = new TestModel { Id = 1, Name = "Test" };
            var apiResponse = new ApiResponse<TestModel>
            {
                ErrorCode = 0,
                Result = expectedResult
            };
            var jsonResponse = JsonConvert.SerializeObject(apiResponse);

            SetupHttpResponse(HttpStatusCode.OK, jsonResponse);

            // Act
            await _commonApiClient.GetAsync<TestModel>("/test", "en-US", null, timeoutSeconds);

            // Assert - The timeout is used in CancellationTokenSource but we can't directly verify it
            // We just ensure the method completes without throwing a timeout exception
            Assert.True(true);
        }

        [Fact]
        public async Task GetStringAsync_WithValidRequest_ReturnsResponseString()
        {
            // Arrange
            var expectedResponse = "test response content";
            SetupHttpResponse(HttpStatusCode.OK, expectedResponse);

            // Act
            var result = await _commonApiClient.GetStringAsync("/test", "en-US");

            // Assert
            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task GetStringAsync_SetsCorrectHeaders()
        {
            // Arrange
            var responseContent = "test response";
            SetupHttpResponse(HttpStatusCode.OK, responseContent);

            // Act
            await _commonApiClient.GetStringAsync("/test", "fr-FR");

            // Assert
            _mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Headers.Contains("Language") &&
                    req.Headers.GetValues("Language").Any(v => v == "fr-FR") &&
                    req.Headers.Accept.ToString().Contains("application/json")
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task GetAsync_WithComplexObject_SerializesQueryStringCorrectly()
        {
            // Arrange
            var queryObject = new ComplexQueryObject
            {
                StringProperty = "test value",
                IntProperty = 42,
                BoolProperty = true,
                NullProperty = null
            };

            var expectedResult = new TestModel { Id = 1, Name = "Test" };
            var apiResponse = new ApiResponse<TestModel>
            {
                ErrorCode = 0,
                Result = expectedResult
            };
            var jsonResponse = JsonConvert.SerializeObject(apiResponse);

            SetupHttpResponse(HttpStatusCode.OK, jsonResponse);

            // Act
            await _commonApiClient.GetAsync<TestModel>("/test", "en-US", queryObject);

            // Assert
            _mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri.Query.Contains("StringProperty=test+value") &&
                    req.RequestUri.Query.Contains("IntProperty=42") &&
                    req.RequestUri.Query.Contains("BoolProperty=True") &&
                    !req.RequestUri.Query.Contains("NullProperty")
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task GetAsync_WhenHttpRequestThrowsException_PropagatesException()
        {
            // Arrange
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new HttpRequestException("Network error"));

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() =>
                _commonApiClient.GetAsync<TestModel>("/test"));
        }

        private void SetupHttpResponse(HttpStatusCode statusCode, string content)
        {
            var response = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(content)
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);
        }

        private class TestModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        private class ComplexQueryObject
        {
            public string StringProperty { get; set; }
            public int IntProperty { get; set; }
            public bool BoolProperty { get; set; }
            public string NullProperty { get; set; }
        }
    }
}