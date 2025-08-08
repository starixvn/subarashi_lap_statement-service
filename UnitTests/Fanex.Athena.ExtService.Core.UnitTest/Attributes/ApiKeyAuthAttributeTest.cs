namespace Fanex.Athena.ExtService.Core.UnitTests.Attributes
{
    using System.Collections.Generic;
    using System.IO;
    using System.Security.Claims;
    using Core.Attributes;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Options;
    using Models.DTO;
    using Moq;
    using Xunit;

    public class ApiKeyAuthAttributeTest
    {
        private readonly Mock<IOptions<List<ApiKey>>> _mockOptions;
        private readonly DefaultHttpContext _httpContext;
        private readonly AuthorizationFilterContext _authorizationContext;

        public ApiKeyAuthAttributeTest()
        {
            _mockOptions = new Mock<IOptions<List<ApiKey>>>();
            _httpContext = new DefaultHttpContext();
            _httpContext.Response.Body = new MemoryStream();

            var modelState = new ModelStateDictionary();
            var actionContext = new ActionContext(
                _httpContext,
                Mock.Of<RouteData>(),
                Mock.Of<ActionDescriptor>(),
                modelState
            );

            _authorizationContext = new AuthorizationFilterContext(
                actionContext,
                new List<IFilterMetadata>()
            );
        }

        [Fact]
        public void OnAuthorization_WhenNoApiKeyHeader_SetsUnauthorizedResult()
        {
            // Arrange
            var apiKeyList = new List<ApiKey>
            {
                new ApiKey { Key = "test-key", SiteName = "TestSite" }
            };
            _mockOptions.Setup(x => x.Value).Returns(apiKeyList);

            var attribute = new ApiKeyAuthAttribute(_mockOptions.Object);

            // Act
            attribute.OnAuthorization(_authorizationContext);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(_authorizationContext.Result);
            var result = _authorizationContext.Result as UnauthorizedObjectResult;
            Assert.Equal("API key is required.", result.Value);
        }

        [Fact]
        public void OnAuthorization_WhenValidApiKey_SetsUserPrincipalAndDoesNotSetResult()
        {
            // Arrange
            var testKey = "valid-key";
            var testSiteName = "TestSite";
            _httpContext.Request.Headers["ApiKey"] = testKey;

            var apiKeyList = new List<ApiKey>
            {
                new ApiKey { Key = testKey, SiteName = testSiteName }
            };
            _mockOptions.Setup(x => x.Value).Returns(apiKeyList);

            var attribute = new ApiKeyAuthAttribute(_mockOptions.Object);

            // Act
            attribute.OnAuthorization(_authorizationContext);

            // Assert
            Assert.Null(_authorizationContext.Result);
            Assert.NotNull(_httpContext.User);
        }

        [Fact]
        public void OnAuthorization_WhenInvalidApiKey_SetsUnauthorizedResult()
        {
            // Arrange
            var testKey = "invalid-key";
            _httpContext.Request.Headers["ApiKey"] = testKey;

            var apiKeyList = new List<ApiKey>
            {
                new ApiKey { Key = "valid-key", SiteName = "TestSite" }
            };
            _mockOptions.Setup(x => x.Value).Returns(apiKeyList);

            var attribute = new ApiKeyAuthAttribute(_mockOptions.Object);

            // Act
            attribute.OnAuthorization(_authorizationContext);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(_authorizationContext.Result);
            var result = _authorizationContext.Result as UnauthorizedObjectResult;
            Assert.Equal("API key is invalid.", result.Value);
        }

        [Fact]
        public void OnAuthorization_WhenApiKeyListIsNull_AllowsAccess()
        {
            // Arrange
            var testKey = "any-key";
            _httpContext.Request.Headers["ApiKey"] = testKey;

            _mockOptions.Setup(x => x.Value).Returns((List<ApiKey>)null);

            var attribute = new ApiKeyAuthAttribute(_mockOptions.Object);

            // Act
            attribute.OnAuthorization(_authorizationContext);

            // Assert
            Assert.Null(_authorizationContext.Result);
        }

        [Fact]
        public void OnAuthorization_WhenApiKeyListIsEmpty_AllowsAccess()
        {
            // Arrange
            var testKey = "any-key";
            _httpContext.Request.Headers["ApiKey"] = testKey;

            var apiKeyList = new List<ApiKey>();
            _mockOptions.Setup(x => x.Value).Returns(apiKeyList);

            var attribute = new ApiKeyAuthAttribute(_mockOptions.Object);

            // Act
            attribute.OnAuthorization(_authorizationContext);

            // Assert
            Assert.Null(_authorizationContext.Result);
        }

        [Fact]
        public void OnAuthorization_WhenValidApiKey_CreatesCorrectClaims()
        {
            // Arrange
            var testKey = "valid-key";
            var testSiteName = "TestSite";
            _httpContext.Request.Headers["ApiKey"] = testKey;

            var apiKeyList = new List<ApiKey>
            {
                new ApiKey { Key = testKey, SiteName = testSiteName }
            };
            _mockOptions.Setup(x => x.Value).Returns(apiKeyList);

            var attribute = new ApiKeyAuthAttribute(_mockOptions.Object);

            // Act
            attribute.OnAuthorization(_authorizationContext);

            // Assert
            Assert.NotNull(_httpContext.User);
            Assert.NotNull(_httpContext.User.Identity);
            var nameClaim = _httpContext.User.FindFirst(ClaimTypes.Name);
            Assert.NotNull(nameClaim);
            Assert.Equal(testSiteName, nameClaim.Value);
        }

        [Theory]
        [InlineData("key1", "Site1")]
        [InlineData("key2", "Site2")]
        [InlineData("special-key-123", "Special Site")]
        public void OnAuthorization_WithMultipleValidKeys_AuthorizesCorrectly(string apiKey, string expectedSiteName)
        {
            // Arrange
            _httpContext.Request.Headers["ApiKey"] = apiKey;

            var apiKeyList = new List<ApiKey>
            {
                new ApiKey { Key = "key1", SiteName = "Site1" },
                new ApiKey { Key = "key2", SiteName = "Site2" },
                new ApiKey { Key = "special-key-123", SiteName = "Special Site" }
            };
            _mockOptions.Setup(x => x.Value).Returns(apiKeyList);

            var attribute = new ApiKeyAuthAttribute(_mockOptions.Object);

            // Act
            attribute.OnAuthorization(_authorizationContext);

            // Assert
            Assert.Null(_authorizationContext.Result);
            Assert.NotNull(_httpContext.User);
            var nameClaim = _httpContext.User.FindFirst(ClaimTypes.Name);
            Assert.NotNull(nameClaim);
            Assert.Equal(expectedSiteName, nameClaim.Value);
        }
    }
}