using Fanex.Athena.ExtService.Report.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Fanex.Athena.ExtService.Report.UnitTests.Controllers
{
    public class ReportBaseControllerTest
    {
        [Fact]
        public void Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var controller = new ReportBaseController();

            // Assert
            Assert.NotNull(controller);
        }
    }
}