using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Fanex.Athena.ExtService.Core.Middleware;

namespace Fanex.Athena.ExtService.Core.UnitTests.Services
{
    public class WhoAmIHealthCheckTest
    {
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly Mock<IWebHostEnvironment> _mockWebHostEnvironment;
        private readonly WhoAmIHealthCheck _healthCheck;

        public WhoAmIHealthCheckTest()
        {
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            _healthCheck = new WhoAmIHealthCheck(_mockHttpContextAccessor.Object, _mockWebHostEnvironment.Object);
        }

        [Fact]
        public async Task CheckHealthAsync_ShouldReturnHealthyResult()
        {
            // Arrange
            var mockHttpContext = new Mock<HttpContext>();
            var mockConnection = new Mock<ConnectionInfo>();
            var mockRequest = new Mock<HttpRequest>();

            _mockWebHostEnvironment.Setup(x => x.EnvironmentName).Returns("Test");
            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext.Object);
            mockHttpContext.Setup(x => x.Connection).Returns(mockConnection.Object);
            mockHttpContext.Setup(x => x.Request).Returns(mockRequest.Object);
            
            mockConnection.Setup(x => x.LocalIpAddress).Returns(System.Net.IPAddress.Loopback);
            mockConnection.Setup(x => x.LocalPort).Returns(80);
            mockConnection.Setup(x => x.RemoteIpAddress).Returns(System.Net.IPAddress.Loopback);
            mockRequest.Setup(x => x.Host).Returns(new HostString("localhost"));
            mockRequest.Setup(x => x.Scheme).Returns("http");

            var context = new HealthCheckContext();

            // Act
            var result = await _healthCheck.CheckHealthAsync(context, CancellationToken.None);

            // Assert
            Assert.Equal(HealthStatus.Healthy, result.Status);
            Assert.Equal("Server Info", result.Description);
            Assert.NotNull(result.Data);
            Assert.True(result.Data.ContainsKey("Env"));
            Assert.Equal("Test", result.Data["Env"]);
        }
    }
}