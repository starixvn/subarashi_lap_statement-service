namespace Fanex.Athena.ExtService.Core.UnitTests.Middleware
{
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;
    using Core.Middleware;
    using Microsoft.AspNetCore.Http;
    using Moq;
    using ONEbook.UM.Models;
    using ONEbook.UM.Services;
    using Xunit;

    public class UnderMaintenanceHandlerTest
    {
        private readonly Mock<RequestDelegate> _mockNext;
        private readonly Mock<IMaintenanceService> _mockMaintenanceService;
        private readonly UnderMaintenanceHandler _handler;

        public UnderMaintenanceHandlerTest()
        {
            _mockNext = new Mock<RequestDelegate>();
            _mockMaintenanceService = new Mock<IMaintenanceService>();
            _handler = new UnderMaintenanceHandler(_mockNext.Object, _mockMaintenanceService.Object);
        }

        [Fact]
        public async Task Invoke_WhenUnderMaintenanceIsFalse_ShouldCallNext()
        {
            // Arrange
            var context = new DefaultHttpContext();
            _mockMaintenanceService.Setup(x => x.IsUnderMaintenanceAsync()).ReturnsAsync(false);

            // Act
            await _handler.Invoke(context);

            // Assert
            _mockNext.Verify(next => next(context), Times.Once);
        }

        [Fact]
        public async Task Invoke_WhenUnderMaintenanceIsTrue_ShouldReturnServiceUnavailable()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            _mockMaintenanceService.Setup(x => x.IsUnderMaintenanceAsync()).ReturnsAsync(true);
            
            // Mock the maintenance time - using object to avoid type issues
            _mockMaintenanceService.Setup(x => x.GetUnderMaintenanceTimeAsync())
                                  .ReturnsAsync(new UnderMaintenanceTime
                                  {
                                      From = "2023-01-01 10:00",
                                      To = "2023-01-01 12:00"
                                  });

            // Act
            await _handler.Invoke(context);

            // Assert
            Assert.Equal((int)HttpStatusCode.ServiceUnavailable, context.Response.StatusCode);
            _mockNext.Verify(next => next(context), Times.Never);
        }
    }
}