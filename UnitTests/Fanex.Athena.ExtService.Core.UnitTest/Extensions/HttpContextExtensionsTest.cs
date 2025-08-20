namespace Fanex.Athena.ExtService.Core.UnitTests.Extensions
{
    using System.ComponentModel.DataAnnotations;
    using System.Net;
    using Core.Attributes;
    using Core.Extensions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features;
    using Moq;
    using Xunit;

    public class HttpContextExtensionsTest
    {
        private readonly Mock<HttpContext> _mockHttpContext;
        private readonly Mock<HttpRequest> _mockRequest;
        private readonly Mock<ConnectionInfo> _mockConnection;
        private readonly Mock<IFeatureCollection> _mockFeatures;

        public HttpContextExtensionsTest()
        {
            _mockHttpContext = new Mock<HttpContext>();
            _mockRequest = new Mock<HttpRequest>();
            _mockConnection = new Mock<ConnectionInfo>();
            _mockFeatures = new Mock<IFeatureCollection>();

            _mockHttpContext.Setup(x => x.Request).Returns(_mockRequest.Object);
            _mockHttpContext.Setup(x => x.Connection).Returns(_mockConnection.Object);
            _mockHttpContext.Setup(x => x.Features).Returns(_mockFeatures.Object);
        }

        [Theory]
        [InlineData("/api/test", "192.168.1.1", "/api/test_192.168.1.1")]
        [InlineData("/users", "10.0.0.1", "/users_10.0.0.1")]
        [InlineData("/", "127.0.0.1", "/_127.0.0.1")]
        public void GetCustomerKey_WithPathAndIP_ReturnsFormattedKey(string path, string ipAddress, string expectedKey)
        {
            // Arrange
            _mockRequest.Setup(x => x.Path).Returns(new PathString(path));
            _mockConnection.Setup(x => x.RemoteIpAddress).Returns(IPAddress.Parse(ipAddress));

            // Act
            var result = _mockHttpContext.Object.GetCustomerKey();

            // Assert
            Assert.Equal(expectedKey, result);
        }

        [Fact]
        public void GetCustomerKey_WithEmptyPath_ReturnsKeyWithEmptyPathAndIP()
        {
            // Arrange
            _mockRequest.Setup(x => x.Path).Returns(PathString.Empty);
            _mockConnection.Setup(x => x.RemoteIpAddress).Returns(IPAddress.Parse("192.168.1.1"));

            // Act
            var result = _mockHttpContext.Object.GetCustomerKey();

            // Assert
            Assert.Equal("_192.168.1.1", result);
        }

        [Fact]
        public void GetCustomerKey_WithLongPath_ReturnsCorrectKey()
        {
            // Arrange
            var longPath = "/api/v1/users/123/orders/456/details";
            var ipAddress = "203.0.113.195";
            var expectedKey = $"{longPath}_{ipAddress}";

            _mockRequest.Setup(x => x.Path).Returns(new PathString(longPath));
            _mockConnection.Setup(x => x.RemoteIpAddress).Returns(IPAddress.Parse(ipAddress));

            // Act
            var result = _mockHttpContext.Object.GetCustomerKey();

            // Assert
            Assert.Equal(expectedKey, result);
        }

        [Fact]
        public void TryGetRateLimitAttribute_WhenAttributeExists_ReturnsTrueAndAttribute()
        {
            // Arrange
            var rateLimitAttribute = new RateLimitAttribute { MaxRequests = 100, TimeWindowInSeconds = 60 };
            var mockEndpoint = new Mock<Endpoint>(null, new EndpointMetadataCollection(rateLimitAttribute), null);
            var endpointFeature = new Mock<IEndpointFeature>();
            endpointFeature.Setup(f => f.Endpoint).Returns(mockEndpoint.Object);
            _mockFeatures.Setup(f => f.Get<IEndpointFeature>()).Returns(endpointFeature.Object);

            // Act
            var result = _mockHttpContext.Object.TryGetRateLimitAttribute(out var retrievedAttribute);

            // Assert
            Assert.True(result);
            Assert.NotNull(retrievedAttribute);
            Assert.Same(rateLimitAttribute, retrievedAttribute);
        }

        [Fact]
        public void TryGetRateLimitAttribute_WhenAttributeDoesNotExist_ReturnsFalseAndNull()
        {
            // Arrange
            var mockEndpoint = new Mock<Endpoint>(null, new EndpointMetadataCollection(), null);
            var endpointFeature = new Mock<IEndpointFeature>();
            endpointFeature.Setup(f => f.Endpoint).Returns(mockEndpoint.Object);
            _mockFeatures.Setup(f => f.Get<IEndpointFeature>()).Returns(endpointFeature.Object);

            // Act
            var result = _mockHttpContext.Object.TryGetRateLimitAttribute(out var retrievedAttribute);

            // Assert
            Assert.False(result);
            Assert.Null(retrievedAttribute);
        }

        [Fact]
        public void TryGetRateLimitAttribute_WhenEndpointIsNull_ReturnsFalseAndNull()
        {
            // Arrange
            var endpointFeature = new Mock<IEndpointFeature>();
            endpointFeature.Setup(f => f.Endpoint).Returns((Endpoint)null);
            _mockFeatures.Setup(f => f.Get<IEndpointFeature>()).Returns(endpointFeature.Object);

            // Act
            var result = _mockHttpContext.Object.TryGetRateLimitAttribute(out var retrievedAttribute);

            // Assert
            Assert.False(result);
            Assert.Null(retrievedAttribute);
        }

        [Fact]
        public void TryGetRateLimitAttribute_WhenEndpointHasOtherAttributes_ReturnsFalseAndNull()
        {
            // Arrange
            var otherAttribute = new RequiredAttribute();
            var mockEndpoint = new Mock<Endpoint>(null, new EndpointMetadataCollection(otherAttribute), null);
            var endpointFeature = new Mock<IEndpointFeature>();
            endpointFeature.Setup(f => f.Endpoint).Returns(mockEndpoint.Object);
            _mockFeatures.Setup(f => f.Get<IEndpointFeature>()).Returns(endpointFeature.Object);

            // Act
            var result = _mockHttpContext.Object.TryGetRateLimitAttribute(out var retrievedAttribute);

            // Assert
            Assert.False(result);
            Assert.Null(retrievedAttribute);
        }

        [Fact]
        public void TryGetRateLimitAttribute_WhenEndpointHasMultipleAttributesIncludingRateLimit_ReturnsTrueAndRateLimitAttribute()
        {
            // Arrange
            var rateLimitAttribute = new RateLimitAttribute { MaxRequests = 50, TimeWindowInSeconds = 30 };
            var otherAttribute = new RequiredAttribute();
            var mockEndpoint = new Mock<Endpoint>(null, new EndpointMetadataCollection(otherAttribute, rateLimitAttribute), null);
            var endpointFeature = new Mock<IEndpointFeature>();
            endpointFeature.Setup(f => f.Endpoint).Returns(mockEndpoint.Object);
            _mockFeatures.Setup(f => f.Get<IEndpointFeature>()).Returns(endpointFeature.Object);

            // Act
            var result = _mockHttpContext.Object.TryGetRateLimitAttribute(out var retrievedAttribute);

            // Assert
            Assert.True(result);
            Assert.NotNull(retrievedAttribute);
            Assert.Same(rateLimitAttribute, retrievedAttribute);
        }

        [Theory]
        [InlineData("::1", "/test", "/test_::1")]  // IPv6 loopback
        [InlineData("2001:db8::1", "/api", "/api_2001:db8::1")]  // IPv6 address
        public void GetCustomerKey_WithIPv6Addresses_ReturnsCorrectKey(string ipv6Address, string path, string expectedKey)
        {
            // Arrange
            _mockRequest.Setup(x => x.Path).Returns(new PathString(path));
            _mockConnection.Setup(x => x.RemoteIpAddress).Returns(IPAddress.Parse(ipv6Address));

            // Act
            var result = _mockHttpContext.Object.GetCustomerKey();

            // Assert
            Assert.Equal(expectedKey, result);
        }

        [Fact]
        public void GetCustomerKey_WithSpecialCharactersInPath_ReturnsCorrectKey()
        {
            // Arrange
            var pathWithSpecialChars = "/api/search?q=test&sort=name";
            var ipAddress = "192.168.1.1";
            var expectedKey = $"{pathWithSpecialChars.Replace("?", "%3F")}_{ipAddress}";

            _mockRequest.Setup(x => x.Path).Returns(new PathString(pathWithSpecialChars));
            _mockConnection.Setup(x => x.RemoteIpAddress).Returns(IPAddress.Parse(ipAddress));

            // Act
            var result = _mockHttpContext.Object.GetCustomerKey();

            // Assert
            Assert.Equal(expectedKey, result);
        }

        [Theory]
        [InlineData(10, 5)]
        [InlineData(100, 60)]
        [InlineData(1000, 300)]
        public void TryGetRateLimitAttribute_WithDifferentRateLimitValues_ReturnsCorrectAttribute(int maxRequests, int windowSeconds)
        {
            // Arrange
            var rateLimitAttribute = new RateLimitAttribute { MaxRequests = maxRequests, TimeWindowInSeconds = windowSeconds };
            var mockEndpoint = new Mock<Endpoint>(null, new EndpointMetadataCollection(rateLimitAttribute), null);
            var endpointFeature = new Mock<IEndpointFeature>();
            endpointFeature.Setup(f => f.Endpoint).Returns(mockEndpoint.Object);
            _mockFeatures.Setup(f => f.Get<IEndpointFeature>()).Returns(endpointFeature.Object);

            // Act
            var result = _mockHttpContext.Object.TryGetRateLimitAttribute(out var retrievedAttribute);

            // Assert
            Assert.True(result);
            Assert.NotNull(retrievedAttribute);
            Assert.Same(rateLimitAttribute, retrievedAttribute);
        }
    }
}