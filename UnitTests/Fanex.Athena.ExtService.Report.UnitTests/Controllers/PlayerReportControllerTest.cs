using System;
using System.Threading.Tasks;
using Fanex.Athena.ExtService.Report.Controllers;
using Fanex.Athena.ExtService.Report.Models.MetaModel;
using Fanex.Athena.ExtService.Report.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Fanex.Athena.ExtService.Report.UnitTests.Controllers
{
    public class PlayerReportControllerTest
    {
        private readonly Mock<IPlayerReportService> _mockPlayerReportService;
        private readonly PlayerReportController _controller;

        public PlayerReportControllerTest()
        {
            _mockPlayerReportService = new Mock<IPlayerReportService>();
            _controller = new PlayerReportController(_mockPlayerReportService.Object);
        }

        [Fact]
        public async Task GetPlayerReportByPlatform_WithValidModel_ReturnsOk()
        {
            // Arrange
            var metaModel = new QueryDataGroupByPlatformMetaModel();

            // Act
            var result = await _controller.GetPlayerReportByPlatform(metaModel);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetPlayerReportByProduct_WithValidModel_ReturnsOk()
        {
            // Arrange
            var metaModel = new QueryDataGroupByProductMetaModel();

            // Act
            var result = await _controller.GetPlayerReportByProduct(metaModel);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetPlayerClosingBalance_WithValidModel_ReturnsOk()
        {
            // Arrange
            var metaModel = new QueryClosingBalanceMetaModel();

            // Act
            var result = await _controller.GetPlayerClosingBalance(metaModel);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}