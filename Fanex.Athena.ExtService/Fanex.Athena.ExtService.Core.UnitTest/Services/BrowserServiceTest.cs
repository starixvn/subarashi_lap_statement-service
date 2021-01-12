using Fanex.Athena.ExtService.Core.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;


namespace Fanex.Athena.ExtService.Core.UnitTests.Services
{
    public class BrowserServiceTest
    {
        private readonly Mock<HttpContext> _mockHttpContext;
        private readonly Mock<HttpRequest> _mockRequest;
        private readonly IBrowserService _browserService;

        private const string HTTP_USER_AGENT = "User-Agent";

        public BrowserServiceTest()
        {
            _mockHttpContext = new Mock<HttpContext>();
            _mockRequest = new Mock<HttpRequest>();
            _browserService = new BrowserService();
        }

        [Fact]
        public void IsTablet_WhenCall_AgentInvalid_ReturnFalse()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HTTP_USER_AGENT] = "12";

            _mockRequest.Setup(x => x.Headers).Returns(httpContext.Request.Headers);
            _mockHttpContext.Setup(x => x.Request).Returns(_mockRequest.Object);
            _browserService.SetContext(_mockHttpContext.Object);

            // Action
            var result = _browserService.IsTablet();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsTablet_WhenCall_ReturnFalse()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HTTP_USER_AGENT] = "laptop";

            _mockRequest.Setup(x => x.Headers).Returns(httpContext.Request.Headers);
            _mockHttpContext.Setup(x => x.Request).Returns(_mockRequest.Object);
            _browserService.SetContext(_mockHttpContext.Object);

            // Action
            var result = _browserService.IsTablet();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsTablet_WhenCall_ReturnTrue()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HTTP_USER_AGENT] = "tablet";

            _mockRequest.Setup(x => x.Headers).Returns(httpContext.Request.Headers);
            _mockHttpContext.Setup(x => x.Request).Returns(_mockRequest.Object);
            _browserService.SetContext(_mockHttpContext.Object);

            // Action
            var result = _browserService.IsTablet();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsMobileOrTablet_WhenCall_InvalidAgent_ReturnFalse()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HTTP_USER_AGENT] = "dt";

            _mockRequest.Setup(x => x.Headers).Returns(httpContext.Request.Headers);
            _mockHttpContext.Setup(x => x.Request).Returns(_mockRequest.Object);
            _browserService.SetContext(_mockHttpContext.Object);

            // Action
            var result = _browserService.IsMobileOrTablet();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsMobileOrTablet_WhenCall_IsValidAgent_ReturnTrue()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HTTP_USER_AGENT] = "android";

            _mockRequest.Setup(x => x.Headers).Returns(httpContext.Request.Headers);
            _mockHttpContext.Setup(x => x.Request).Returns(_mockRequest.Object);
            _browserService.SetContext(_mockHttpContext.Object);

            // Action
            var result = _browserService.IsMobileOrTablet();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsMobile_WhenCall_ReturnFalse()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HTTP_USER_AGENT] = "android";

            _mockRequest.Setup(x => x.Headers).Returns(httpContext.Request.Headers);
            _mockHttpContext.Setup(x => x.Request).Returns(_mockRequest.Object);
            _browserService.SetContext(_mockHttpContext.Object);

            // Action
            var result = _browserService.IsMobile();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsMobile_WhenCallIsTablet_ReturnFalse()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HTTP_USER_AGENT] = "silk";

            _mockRequest.Setup(x => x.Headers).Returns(httpContext.Request.Headers);
            _mockHttpContext.Setup(x => x.Request).Returns(_mockRequest.Object);
            _browserService.SetContext(_mockHttpContext.Object);

            // Action
            var result = _browserService.IsMobile();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsMobile_WhenCallOnly_ReturnTrue()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HTTP_USER_AGENT] = "nexus";

            _mockRequest.Setup(x => x.Headers).Returns(httpContext.Request.Headers);
            _mockHttpContext.Setup(x => x.Request).Returns(_mockRequest.Object);
            _browserService.SetContext(_mockHttpContext.Object);

            // Action
            var result = _browserService.IsMobile();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsAndroidNativeBrowser_WhenCall_ReturnTrue()
        {
            // Arrange
            string userAgent = "./1.2 chrome";

            // Action
            var result = _browserService.IsAndroidNativeBrowser(userAgent);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetBrowserInfo_WhenCall_ReturnTrue()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HTTP_USER_AGENT] = "android";

            _mockRequest.Setup(x => x.Headers).Returns(httpContext.Request.Headers);
            _mockHttpContext.Setup(x => x.Request).Returns(_mockRequest.Object);
            _browserService.SetContext(_mockHttpContext.Object);

            // Action
            var result = _browserService.GetBrowserInfo();

            // Assert
            Assert.True(result.IsMobile);
            Assert.True(result.IsTablet);
        }

        [Fact]
        public void GetUserAgent_WhenCall_IsExist_ReturnTrue()
        {
            // Arrange
            string expectedResult = "android";
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HTTP_USER_AGENT] = expectedResult;

            _mockRequest.Setup(x => x.Headers).Returns(httpContext.Request.Headers);
            _mockHttpContext.Setup(x => x.Request).Returns(_mockRequest.Object);
            _browserService.SetContext(_mockHttpContext.Object);

            // Action
            var result = _browserService.GetUserAgent();

            // Assert
            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public void GetUserAgent_WhenCall_IsNotExist_ReturnTrue()
        {
            // Arrange
            string expectedResult = "N/A";

            // Action
            var result = _browserService.GetUserAgent();

            // Assert
            Assert.Equal(result, expectedResult);
        }
    }
}
