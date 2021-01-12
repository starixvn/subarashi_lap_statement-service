using Fanex.Athena.ExtService.Core.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.ExtService.Core.UnitTests.Middleware
{
    public class WhoAmIHealthCheckTest
    {
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;

        public WhoAmIHealthCheckTest()
        {
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
        }

        [Fact]
        public async Task CheckHealthAsync_WhenCall_ReturnHealthCheckResult()
        {
            // Arrange
            var connectionInfo = new Mock<ConnectionInfo>();
            var expectedPort = 6969;
            var expectedIPAddress = new IPAddress(0x2414188f);

            connectionInfo.Setup(x => x.RemoteIpAddress).Returns(expectedIPAddress);
            connectionInfo.Setup(x => x.LocalIpAddress).Returns(expectedIPAddress);
            connectionInfo.Setup(x => x.LocalPort).Returns(expectedPort);

            _httpContextAccessor.Setup(x => x.HttpContext.Connection).Returns(connectionInfo.Object);
            var whoAmIHealthCheck = new WhoAmIHealthCheck(_httpContextAccessor.Object);

            // Action
            var healthCheckContext = new HealthCheckContext();
            var result = await whoAmIHealthCheck.CheckHealthAsync(healthCheckContext).ConfigureAwait(false);

            // Assert
            Assert.Equal(result.Data["Local Ip"], expectedIPAddress.ToString());
            Assert.Equal(result.Data["Local Port"].ToString(), expectedPort.ToString());
            Assert.Equal(result.Data["Client Ip"], expectedIPAddress.ToString());
        }
    }
}
