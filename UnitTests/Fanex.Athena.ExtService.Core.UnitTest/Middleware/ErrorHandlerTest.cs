using Assert = Xunit.Assert;

namespace Fanex.Athena.ExtService.Core.UnitTests.Middleware
{
    using System;
    using System.Threading.Tasks;
    using Core.Middleware;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class ErrorHandlerTest
    {
        private readonly Mock<HttpContext> _mockContext;
        private readonly Mock<ILogger<ErrorHandler>> _mockLogger;
        private readonly Mock<RequestDelegate> _next;

        public ErrorHandlerTest()
        {
            _mockContext = new Mock<HttpContext>();
            _mockLogger = new Mock<ILogger<ErrorHandler>>();
            _next = new Mock<RequestDelegate>();
        }

        [Fact]
        public async Task Invoke_WithoutError_InvokeNext()
        {
            //Arrange
            _mockContext.Setup(context => context.Features[typeof(IExceptionHandlerFeature)]).Returns((IExceptionHandlerFeature)null);

            var handler = new ErrorHandler(_next.Object, _mockLogger.Object);

            //Arrange
            await handler.Invoke(_mockContext.Object).ConfigureAwait(false);

            //Assert
            _next.Verify(req => req.Invoke(_mockContext.Object));
        }

        private const int ServerErrorCode = 500;
        private const string ContentType = "application/json";

        [Fact]
        public async Task Invoke_HandleErrorWithException_ResponseError()
        {
            //Arrange
            var context = new DefaultHttpContext();

            _mockContext.Setup(ctx => ctx.Features[typeof(IExceptionHandlerFeature)]).Returns((IExceptionHandlerFeature)null);
            _mockContext.Setup(ctx => ctx.Response).Returns(context.Response);

            _next.Setup(nxt => nxt.Invoke(It.IsAny<HttpContext>())).Throws(new Exception("Any"));

            var handler = new ErrorHandler(_next.Object, _mockLogger.Object);

            //Arrange
            await handler.Invoke(_mockContext.Object).ConfigureAwait(false);

            //Assert
            Assert.Equal(ServerErrorCode, context.Response.StatusCode);
            Assert.Equal(ContentType, context.Response.ContentType);
        }

        [Fact]
        public async Task Invoke_HandleErrorWithError_ResponseError()
        {
            //Arrange
            var context = new DefaultHttpContext();

            var exception = new Mock<IExceptionHandlerFeature>();
            exception.Setup(ex => ex.Error).Returns(new Exception("Any"));
            _mockContext.Setup(ctx => ctx.Features[typeof(IExceptionHandlerFeature)]).Returns(exception.Object);
            _mockContext.Setup(ctx => ctx.Response).Returns(context.Response);

            var handler = new ErrorHandler(_next.Object, _mockLogger.Object);

            //Arrange
            await handler.Invoke(_mockContext.Object).ConfigureAwait(false);

            //Assert
            Assert.Equal(ServerErrorCode, context.Response.StatusCode);
            Assert.Equal(ContentType, context.Response.ContentType);
        }
    }
}