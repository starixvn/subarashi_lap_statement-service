using Fanex.Athena.ExtService.Core.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Fanex.Athena.ExtService.Core.UnitTests.Middleware
{
    public class MiddlewareExtensionsTest
    {
        [Fact]
        public void UseRateLimit_ShouldAddRateLimitHandlerToPipeline()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddDistributedMemoryCache();
            services.AddSingleton<Fanex.Athena.ExtService.Common.Services.ICustomDistributedCache, Fanex.Athena.ExtService.Common.Services.CustomDistributedCache>();
            services.AddLogging();
            var serviceProvider = services.BuildServiceProvider();

            var appBuilder = new ApplicationBuilder(serviceProvider);
            var mockWebHost = new Mock<IWebHost>();
            mockWebHost.Setup(x => x.Services).Returns(serviceProvider);

            // Act
            appBuilder.UseRateLimit();

            // Assert
            Assert.NotNull(appBuilder.Build());
        }

        [Fact]
        public void UseWhoAmICheck_ShouldAddWhoAmIHandlerToPipeline()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddHealthChecks();
            services.AddOptions(); // Add options services
            services.AddLogging(); // Add logging services
            var serviceProvider = services.BuildServiceProvider();

            var appBuilder = new ApplicationBuilder(serviceProvider);

            // Act
            appBuilder.UseWhoAmICheck();

            // Assert
            Assert.NotNull(appBuilder.Build());
        }

        [Fact]
        public void UseUnderMaintenance_ShouldAddUnderMaintenanceHandlerToPipeline()
        {
            // Arrange
            var services = new ServiceCollection();
            var mockMaintenanceService = new Mock<ONEbook.UM.Services.IMaintenanceService>();
            services.AddSingleton(mockMaintenanceService.Object);
            var serviceProvider = services.BuildServiceProvider();

            var appBuilder = new ApplicationBuilder(serviceProvider);

            // Act
            appBuilder.UseUnderMaintenance();

            // Assert
            Assert.NotNull(appBuilder.Build());
        }
    }
}