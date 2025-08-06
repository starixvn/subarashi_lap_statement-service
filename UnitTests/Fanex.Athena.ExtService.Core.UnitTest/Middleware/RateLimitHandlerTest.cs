using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Fanex.Athena.ExtService.Common.Services;
using Fanex.Athena.ExtService.Core.Attributes;
using Fanex.Athena.ExtService.Core.Extensions;
using Fanex.Athena.ExtService.Core.Helper;
using Fanex.Athena.ExtService.Core.Middleware;
using Fanex.Athena.ExtService.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Xunit;

namespace Fanex.Athena.ExtService.Core.UnitTests.Middleware
{
    public class RateLimitHandlerTest
    {
        private readonly Mock<RequestDelegate> _mockNext;
        private readonly Mock<ICustomDistributedCache> _mockCache;
        private readonly RateLimitHandler _handler;

        public RateLimitHandlerTest()
        {
            _mockNext = new Mock<RequestDelegate>();
            _mockCache = new Mock<ICustomDistributedCache>();
            _handler = new RateLimitHandler(_mockNext.Object, _mockCache.Object);
        }

        [Fact]
        public async Task InvokeAsync_WhenRateLimitAttributeIsMissing_ShouldCallNext()
        {
            // Arrange
            var context = new DefaultHttpContext();

            // Act
            await _handler.InvokeAsync(context);

            // Assert
            _mockNext.Verify(next => next(context), Times.Once);
        }

        [Fact]
        public async Task InvokeAsync_FirstRequest_ShouldAllowAndSetCache()
        {
            // Arrange
            var context = CreateContextWithRateLimitAttribute(5, 10);
            _mockCache.Setup(cache => cache.GetAsync<ConsumptionData>(It.IsAny<string>(), default))
                     .ReturnsAsync((ConsumptionData)null);

            // Act
            await _handler.InvokeAsync(context);

            // Assert
            _mockNext.Verify(next => next(context), Times.Once);
            _mockCache.Verify(cache => cache.SetAsync(It.IsAny<string>(), It.IsAny<ConsumptionData>(), It.IsAny<DistributedCacheEntryOptions>(), default), Times.Once);
        }

        [Fact]
        public async Task InvokeAsync_WithinRateLimit_ShouldAllowAndUpdateCache()
        {
            // Arrange
            var context = CreateContextWithRateLimitAttribute(5, 10);
            var consumptionData = new ConsumptionData(DateTimeHelper.UtcNow.AddSeconds(-30), 2);
            
            _mockCache.Setup(cache => cache.GetAsync<ConsumptionData>(It.IsAny<string>(), default))
                     .ReturnsAsync(consumptionData);

            // Act
            await _handler.InvokeAsync(context);

            // Assert
            _mockNext.Verify(next => next(context), Times.Once);
            _mockCache.Verify(cache => cache.SetAsync(It.IsAny<string>(), It.IsAny<ConsumptionData>(), It.IsAny<DistributedCacheEntryOptions>(), default), Times.Once);
        }

        [Fact]
        public async Task InvokeAsync_ExceedsRateLimit_ShouldReturn429StatusCode()
        {
            // Arrange
            var context = CreateContextWithRateLimitAttribute(1, 10);
            var consumptionData = new ConsumptionData(DateTimeHelper.UtcNow, 1);
            
            _mockCache.Setup(cache => cache.GetAsync<ConsumptionData>(It.IsAny<string>(), default))
                     .ReturnsAsync(consumptionData);

            // Act
            await _handler.InvokeAsync(context);

            // Assert
            Assert.Equal((int)HttpStatusCode.TooManyRequests, context.Response.StatusCode);
            Assert.Equal("application/json", context.Response.ContentType);
            _mockNext.Verify(next => next(context), Times.Never);
        }

        private static DefaultHttpContext CreateContextWithRateLimitAttribute(int maxRequests, int timeWindowInSeconds)
        {
            var context = new DefaultHttpContext();
            var rateLimitAttribute = new RateLimitAttribute
            {
                MaxRequests = maxRequests,
                TimeWindowInSeconds = timeWindowInSeconds
            };

            var endpoint = new Endpoint(null, new EndpointMetadataCollection(rateLimitAttribute), null);
            var endpointFeature = new Mock<IEndpointFeature>();
            endpointFeature.Setup(f => f.Endpoint).Returns(endpoint);
            context.Features.Set(endpointFeature.Object);

            context.Response.Body = new MemoryStream();
            
            return context;
        }
    }
}