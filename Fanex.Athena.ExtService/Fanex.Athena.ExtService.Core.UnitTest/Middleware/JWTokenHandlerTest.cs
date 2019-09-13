using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fanex.Athena.ExtService.Core.Middleware;
using Fanex.Athena.ExtService.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Moq;
using Xunit;

namespace Fanex.Athena.ExtService.Core.UnitTests.Middleware
{
    public class JWTokenHandlerTest
    {
        private readonly Mock<HttpContext> _mockContext;
        private readonly Mock<HttpRequest> _mockRequest;
        private readonly Mock<IQueryCollection> _mockQueryCollection;
        private readonly Mock<IEncryptService> _mockEncryptService;
        private readonly Mock<IHeaderDictionary> _mockHeader;
        const string Token = "ANYTOKEN";
        const string DecryptedToken = "ANYDECRYPTEDTOKEN";
        public JWTokenHandlerTest()
        {
            _mockContext = new Mock<HttpContext>();
            _mockRequest = new Mock<HttpRequest>();
            _mockQueryCollection = new Mock<IQueryCollection>();
            _mockEncryptService = new Mock<IEncryptService>();
            _mockEncryptService.Setup(x => x.Decrypt(It.IsAny<string>())).Returns(DecryptedToken);
            _mockHeader = new Mock<IHeaderDictionary>();
        }
        [Fact]
        public async Task Invoke_WithoutTokenHeader_DecryptAndAddTokenToHeaderAsync()
        {
            //Arrange
            StringValues actualToken = Token;

            _mockHeader.Setup(x => x.ContainsKey("Authorization")).Returns(false);

            _mockQueryCollection.Setup(x => x.TryGetValue(It.IsAny<string>(), out actualToken)).Returns(true);

            _mockRequest.Setup(x => x.Headers).Returns(_mockHeader.Object);
            _mockRequest.Setup(x => x.Query).Returns(_mockQueryCollection.Object);

            _mockContext.Setup(x => x.Request).Returns(_mockRequest.Object);
            var handler = new JWTokenHandler(next: async (context) => { await Task.CompletedTask; }, encryptService: _mockEncryptService.Object);

            //Arrange
            await handler.Invoke(_mockContext.Object).ConfigureAwait(false);

            //Assert
            _mockRequest.Verify(x => x.Headers.Add("Authorization", "Bearer " + DecryptedToken), Times.Once);
        }

        [Fact]
        public async Task Invoke_AlreadyHasTokenInHeader_DecryptAndAddTokenToHeader()
        {
            //Arrange
            StringValues actualToken = Token;
            var mockHeader = new Mock<IHeaderDictionary>();
            mockHeader.Setup(x => x["Authorization"]).Returns(actualToken);
            mockHeader.Setup(x => x.ContainsKey("Authorization")).Returns(true);
            _mockRequest.Setup(x => x.Headers).Returns(mockHeader.Object);
            _mockRequest.Setup(x => x.Query).Returns(_mockQueryCollection.Object);

            _mockContext.Setup(x => x.Request).Returns(_mockRequest.Object);
            var handler = new JWTokenHandler(next: async (context) => { await Task.CompletedTask; }, encryptService: _mockEncryptService.Object);

            //Arrange
            await handler.Invoke(_mockContext.Object).ConfigureAwait(false);

            //Assert
            _mockRequest.Verify(x => x.Headers.Remove("Authorization"), Times.Once);
            _mockRequest.Verify(x => x.Headers.Add("Authorization", "Bearer " + DecryptedToken), Times.Once);
        }

        [Fact]
        public async Task Invoke_InValidTokenInHeader_NotAddTokenToHeaderAsync()
        {
            //Arrange
            var mockHeader = new Mock<IHeaderDictionary>();

            mockHeader.Setup(x => x.ContainsKey("Authorization")).Returns(true);
            mockHeader.Setup(x => x["Authorization"]).Returns(string.Empty);
            _mockRequest.Setup(x => x.Headers).Returns(mockHeader.Object);
            _mockRequest.Setup(x => x.Query).Returns(_mockQueryCollection.Object);

            _mockContext.Setup(x => x.Request).Returns(_mockRequest.Object);
            var handler = new JWTokenHandler(next: async (context) => { await Task.CompletedTask; }, encryptService: _mockEncryptService.Object);

            //Arrange
            await handler.Invoke(_mockContext.Object).ConfigureAwait(false);

            //Assert
            _mockRequest.Verify(x => x.Headers.Add("Authorization", "Bearer " + DecryptedToken), Times.Never);
        }

        [Fact]
        public async Task Invoke_WithoutToken_NotAddTokenToHeaderAsync()
        {
            //Arrange
            var mockHeader = new Mock<IHeaderDictionary>();

            mockHeader.Setup(x => x.ContainsKey("Authorization")).Returns(false);
            _mockRequest.Setup(x => x.Headers).Returns(mockHeader.Object);
            _mockRequest.Setup(x => x.Query).Returns(_mockQueryCollection.Object);

            _mockContext.Setup(x => x.Request).Returns(_mockRequest.Object);
            var handler = new JWTokenHandler(next: async (context) => { await Task.CompletedTask; }, encryptService: _mockEncryptService.Object);

            //Arrange
            await handler.Invoke(_mockContext.Object).ConfigureAwait(false);

            //Assert
            _mockRequest.Verify(x => x.Headers.Add("Authorization", "Bearer " + DecryptedToken), Times.Never);
        }
    }
}
