using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fanex.Athena.ExtService.Report.Models.MetaModel;
using Fanex.Athena.ExtService.Report.Models.ResultModel;
using Fanex.Athena.ExtService.Report.Services;
using Xunit;

namespace Fanex.Athena.ExtService.Report.UnitTests.Services
{
    public class PlayerReportServiceTest
    {
        private readonly PlayerReportService _service;

        public PlayerReportServiceTest()
        {
            _service = new PlayerReportService();
        }

        [Fact]
        public async Task GetPlayerReportByPlatform_ShouldReturnEmptyResult()
        {
            // Arrange
            var metaModel = new QueryDataGroupByPlatformMetaModel();

            // Act
            var result = await _service.GetPlayerReportByPlatform(metaModel);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ReportResultWithBaseCurrencyModel<object>>(result);
        }

        [Fact]
        public async Task GetPlayerReportByProduct_ShouldReturnEmptyResult()
        {
            // Arrange
            var metaModel = new QueryDataGroupByProductMetaModel();

            // Act
            var result = await _service.GetPlayerReportByProduct(metaModel);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ReportResultWithBaseCurrencyModel<object>>(result);
        }

        [Fact]
        public async Task GetPlayerClosingBalance_ShouldReturnEmptyResult()
        {
            // Arrange
            var metaModel = new QueryClosingBalanceMetaModel();

            // Act
            var result = await _service.GetPlayerClosingBalance(metaModel);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ReportResultBaseModel<object>>(result);
        }
    }
}