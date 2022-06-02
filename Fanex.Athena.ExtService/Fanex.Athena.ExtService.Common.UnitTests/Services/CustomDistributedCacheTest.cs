using Fanex.Athena.ExtService.Common.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.ExtService.Common.UnitTests.Services
{
    public class CustomDistributedCacheTest
    {
        private readonly Mock<IDistributedCache> _distributedCache;

        public CustomDistributedCacheTest()
        {
            _distributedCache = new Mock<IDistributedCache>();
        }

        [Fact]
        public async Task Invoke_RemoveAsync()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CustomDistributedCache>>();
            var expectedTask = Task.FromResult("expected result");
            _distributedCache.Setup(x => x.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(expectedTask);
            var customDistributedCache = new CustomDistributedCache(_distributedCache.Object, mockLogger.Object);

            // Action
            await customDistributedCache.RemoveAsync("key");

            // Assert
            _distributedCache.Verify(m => m.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Invoke_RemoveAsync_Exception_WriteLog()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CustomDistributedCache>>();
            var expectedTask = Task.FromException(new Exception("my exception"));
            _distributedCache.Setup(x => x.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(expectedTask);
            var customDistributedCache = new CustomDistributedCache(_distributedCache.Object, mockLogger.Object);

            // Action
            await customDistributedCache.RemoveAsync("key");

            // Assert
            _distributedCache.Verify(m => m.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            
            mockLogger.Verify(r => r.Log(
                LogLevel.Critical, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);
        }

        [Fact]
        public async Task Invoke_SetAsync_Exception_WriteLog()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CustomDistributedCache>>();
            var expectedTask = Task.FromException(new Exception("my exception"));
            _distributedCache.Setup(x => x.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>())).Returns(expectedTask).Verifiable();

            var customDistributedCache = new CustomDistributedCache(_distributedCache.Object, mockLogger.Object);

            // Action
            await customDistributedCache.SetAsync("key", "123", new DistributedCacheEntryOptions());

            // Assert
            _distributedCache.Verify(m => m.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.Once);
            mockLogger.Verify(r =>
            r.Log(LogLevel.Critical, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);
        }
    }
}
