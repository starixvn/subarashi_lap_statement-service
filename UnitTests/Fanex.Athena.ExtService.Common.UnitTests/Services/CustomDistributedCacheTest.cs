namespace Fanex.Athena.ExtService.Common.UnitTests.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Services;
    using MessagePack;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Logging;
    using Moq;
    using UnitTest.Common.Helpers;
    using Xunit;

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
            _distributedCache.Setup(x => x.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                             .Returns(expectedTask);
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
            _distributedCache.Setup(x => x.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                             .Returns(expectedTask);
            var customDistributedCache = new CustomDistributedCache(_distributedCache.Object, mockLogger.Object);

            // Action
            await customDistributedCache.RemoveAsync("key");

            // Assert
            _distributedCache.Verify(m => m.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);

            mockLogger.Verify(r => r.Log(
                                         LogLevel.Critical, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
                                         It.IsAny<Exception>(),
                                         (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                              Times.Once);
        }

        [Fact]
        public async Task Invoke_SetAsync_Exception_WriteLog()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CustomDistributedCache>>();
            var expectedTask = Task.FromException(new Exception("my exception"));
            _distributedCache
               .Setup(x => x.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(),
                                      It.IsAny<CancellationToken>())).Returns(expectedTask).Verifiable();

            var customDistributedCache = new CustomDistributedCache(_distributedCache.Object, mockLogger.Object);

            // Action
            await customDistributedCache.SetAsync("key", "123", new DistributedCacheEntryOptions());

            // Assert
            _distributedCache
               .Verify(m => m.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()),
                       Times.Once);
            mockLogger.Verify(r =>
                                  r.Log(LogLevel.Critical, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
                                        It.IsAny<Exception>(),
                                        (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                              Times.Once);
        }

        [Fact]
        public async Task Invoke_RefreshAsync()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CustomDistributedCache>>();
            var expectedTask = Task.FromResult("expected result");
            _distributedCache.Setup(x => x.RefreshAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                             .Returns(expectedTask);
            var customDistributedCache = new CustomDistributedCache(_distributedCache.Object, mockLogger.Object);

            // Action
            await customDistributedCache.RefreshAsync("key");

            // Assert
            _distributedCache.Verify(m => m.RefreshAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Invoke_RefreshAsync_Exception_WriteLog()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CustomDistributedCache>>();
            var expectedTask = Task.FromException(new Exception("my exception"));
            _distributedCache.Setup(x => x.RefreshAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                             .Returns(expectedTask);
            var customDistributedCache = new CustomDistributedCache(_distributedCache.Object, mockLogger.Object);

            // Action
            await customDistributedCache.RefreshAsync("key");

            // Assert
            _distributedCache.Verify(m => m.RefreshAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);

            mockLogger.Verify(r => r.Log(
                                         LogLevel.Critical, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
                                         It.IsAny<Exception>(),
                                         (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                              Times.Once);
        }

        [Fact]
        public async Task Invoke_GetAsyncT_EmptySource_ReturnDefaultT()
        {
            // Arrange
            var expectedTask = Task.FromResult<byte[]>(null);
            _distributedCache.Setup(x => x.GetAsync(It.IsAny<string>(),
                                                    It.IsAny<CancellationToken>()))
                             .Returns(expectedTask)
                             .Verifiable();
            var mockLogger = new Mock<ILogger<CustomDistributedCache>>();
            var customDistributedCache = new CustomDistributedCache(_distributedCache.Object, mockLogger.Object);

            // Action
            var result = await customDistributedCache.GetAsync<SampleObject>(It.IsAny<string>());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Invoke_GetAsyncT_WithSource_ReturnT()
        {
            // Arrange
            var value = FakeHelper.Create<SampleObject>();
            var expectedTask = Task.FromResult(MessagePackSerializer.Serialize(value));
            _distributedCache.Setup(x => x.GetAsync(It.IsAny<string>(),
                                                    It.IsAny<CancellationToken>()))
                             .Returns(expectedTask)
                             .Verifiable();
            var mockLogger = new Mock<ILogger<CustomDistributedCache>>();
            var customDistributedCache = new CustomDistributedCache(_distributedCache.Object, mockLogger.Object);

            // Action
            var result = await customDistributedCache.GetAsync<SampleObject>(It.IsAny<string>());

            // Assert
            Assert.IsType<SampleObject>(result);
        }
        [MessagePackObject]
        public class SampleObject
        {
            [Key(0)]
            public int Id { get; set; }

            [Key(1)]
            public string Name { get; set; }
        }
    }
}