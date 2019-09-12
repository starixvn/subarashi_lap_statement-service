using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fanex.Athena.ExtService.Core.Middleware;
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
        public JWTokenHandlerTest()
        {
            _mockContext = new Mock<HttpContext>();
            _mockRequest = new Mock<HttpRequest>();
            _mockQueryCollection = new Mock<IQueryCollection>();
        }
        [Fact]
        public async Task Invoke_WithoutTokenHeader_AddTokenToHeaderAsync()
        {
            //Arrange
            const string Token = "ANYTOKEN";
            StringValues actualToken = Token;

            _mockQueryCollection.Setup(x => x.TryGetValue(It.IsAny<string>(), out actualToken)).Returns(true);

            _mockRequest.Setup(x => x.Headers).Returns(new Mock<IHeaderDictionary>().Object);
            _mockRequest.Setup(x => x.Query).Returns(_mockQueryCollection.Object);

            _mockContext.Setup(x => x.Request).Returns(_mockRequest.Object);
            var handler = new JWTokenHandler(next: async (context) => { await Task.CompletedTask; });

            //Arrange
            await handler.Invoke(_mockContext.Object).ConfigureAwait(false);

            //Assert
            _mockRequest.Verify(x => x.Headers.Add("Authorization", "Bearer " + Token), Times.Once);
        }

        [Fact]
        public async Task Invoke_AlreadyHasTokenInHeader_NotAddTokenToHeaderAsync()
        {
            //Arrange
            const string Token = "ANYTOKEN";
            StringValues actualToken = Token;

            _mockQueryCollection.Setup(x => x.TryGetValue(It.IsAny<string>(), out actualToken)).Returns(true);

            var mockHeader = new Mock<IHeaderDictionary>();
            mockHeader.Setup(x => x.ContainsKey("Authorization")).Returns(true);
            _mockRequest.Setup(x => x.Headers).Returns(mockHeader.Object);
            _mockRequest.Setup(x => x.Query).Returns(_mockQueryCollection.Object);

            _mockContext.Setup(x => x.Request).Returns(_mockRequest.Object);
            var handler = new JWTokenHandler(next: async (context) => { await Task.CompletedTask; });

            //Arrange
            await handler.Invoke(_mockContext.Object).ConfigureAwait(false);

            //Assert
            _mockRequest.Verify(x => x.Headers.Add("Authorization", "Bearer " + Token), Times.Never);
        }

        [Fact]
        public async Task Invoke_DontHaveValidToken_NotAddTokenToHeaderAsync()
        {
            //Arrange
            StringValues actualToken = string.Empty;

            _mockQueryCollection.Setup(x => x.TryGetValue(It.IsAny<string>(), out actualToken)).Returns(true);

            _mockRequest.Setup(x => x.Headers).Returns(new Mock<IHeaderDictionary>().Object);
            _mockRequest.Setup(x => x.Query).Returns(_mockQueryCollection.Object);

            _mockContext.Setup(x => x.Request).Returns(_mockRequest.Object);
            var handler = new JWTokenHandler(next: async (context) => { await Task.CompletedTask; });

            //Arrange
            await handler.Invoke(_mockContext.Object).ConfigureAwait(false);

            //Assert
            _mockRequest.Verify(x => x.Headers.Add("Authorization", "Bearer " + actualToken), Times.Never);
        }
    }
}
