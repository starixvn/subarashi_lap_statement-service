using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

using Fanex.Athena.ExtService.Common.Models.DTO;
using Fanex.Athena.ExtService.Common.Services;
using Fanex.Data;

using Microsoft.Extensions.Caching.Memory;

using Moq;

using Xunit;

namespace Fanex.Athena.ExtService.Common.UnitTests.Services
{
    public class CommonServiceTest : IDisposable
    {
        private readonly IMemoryCache _memoryCache;
        public CommonServiceTest()
        {
            DbSettingProviderManager
             .StartTestingSession()
             .Run();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        public void Dispose()
        {
            _memoryCache.Dispose();
        }

        [Fact]
        public async Task GetLicenseeInfoAsync_WhenCall_AssignCorrectParrameterAndReturnCorrectResultAsync()
        {
            //Arrange
            const string SiteName = "12Bet";
            const int CustId = 1;
            const int CurrencyId = 1;
            const int AgentId = 2;
            const string CurrencyName = "USD";
            const bool HasComm = true;
            var mockIDataReader = new Mock<IDataReader>();

            _memoryCache.Remove("__LicenseeExtInfoCacheKey_" + SiteName);

            mockIDataReader.Setup(m => m["CurrencyId"]).Returns(CurrencyId);
            mockIDataReader.Setup(m => m["AgentId"]).Returns(AgentId);
            mockIDataReader.Setup(m => m["CurrencyName"]).Returns(CurrencyName);
            mockIDataReader.Setup(m => m["IsAllowedCustomerComm"]).Returns(HasComm);
            mockIDataReader.Setup(x => x.NextResult()).Returns(true);
            mockIDataReader.SetupSequence(m => m.Read())
              .Returns(true)
              .Returns(false)
              .Returns(true)
              .Returns(false);


            var mockObjectDB = new Mock<IObjectDb>();

            mockObjectDB.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>()))
                .Returns(Task.FromResult(mockIDataReader.Object));

            ObjectDbFactory.SwitchToTestingMode();
            ObjectDbFactory.Mock((x, y) => mockObjectDB.Object);

            var service = new CommonService(_memoryCache);


            ///Action

            var result = await service.GetLicenseeInfoAsync(SiteName, CustId).ConfigureAwait(false);

            ///Assert
            Assert.Equal(result.SiteName, SiteName);
            Assert.Equal(result.IsAllowedCustomerComm, HasComm);
            Assert.Equal(result.Currencies[0].CurrencyId, CurrencyId);
            Assert.Equal(result.Currencies[0].CurrencyName, CurrencyName);
            Assert.Equal(result.Currencies[0].AgentId, AgentId);
            mockObjectDB.Verify(x => x.SetParameterValue("siteName", SiteName), Times.Once);
            mockObjectDB.Verify(x => x.SetParameterValue("custId", CustId), Times.Once);
        }

        [Fact]
        public async Task GetLicenseeInfoAsync_WhenCall_ReturnDataFromCache()
        {
            //Arrange
            const string SiteName = "12Bet";
            const int CustId = 1;
            const int CurrencyId = 1;
            const int AgentId = 2;
            const string CurrencyName = "USD";
            const bool HasComm = true;

            _memoryCache.Set("__LicenseeExtInfoCacheKey_" + SiteName, new LicenseeInfo
            {

                Currencies = new[] { new CurrencyInfo { CurrencyId = CurrencyId, CurrencyName = CurrencyName, AgentId = AgentId } },
                SiteName = SiteName,
                IsAllowedCustomerComm = HasComm
            });

            var service = new CommonService(_memoryCache);

            ///Action

            var result = await service.GetLicenseeInfoAsync(SiteName, CustId).ConfigureAwait(false);

            ///Assert
            Assert.Equal(result.SiteName, SiteName);
            Assert.Equal(result.IsAllowedCustomerComm, HasComm);
            Assert.Equal(result.Currencies[0].CurrencyId, CurrencyId);
            Assert.Equal(result.Currencies[0].CurrencyName, CurrencyName);
            Assert.Equal(result.Currencies[0].AgentId, AgentId);
        }

        [Fact]
        public async Task GetBalanceUpAsync_WhenCall_ReturnDataFromCache()
        {
            //Arrange
            var balanceupInfo = new BalanceUp
            {
                BalanceUpDay = DateTime.Now,
                MoveBetsDay = DateTime.Now
            };
            _memoryCache.Set("EXT_LAP_CACHE_BALANCEUP", balanceupInfo);

            var service = new CommonService(_memoryCache);

            ///Action

            var result = await service.GetBalanceUpAsync().ConfigureAwait(false);

            ///Assert
            Assert.Equal(result, balanceupInfo);

        }

        [Fact]
        public async Task GetBalanceUpAsync_WhenCall_ReturnCorrectResultAsync()
        {
            //Arrange
            var balanceupInfo = new BalanceUp
            {
                BalanceUpDay = DateTime.Now,
                MoveBetsDay = DateTime.Now
            };

            var mockIDataReader = new Mock<IDataReader>();

            _memoryCache.Remove("EXT_LAP_CACHE_BALANCEUP");

            mockIDataReader.Setup(m => m["balanceupday"]).Returns(balanceupInfo.BalanceUpDay);
            mockIDataReader.Setup(m => m["movebetsday"]).Returns(balanceupInfo.MoveBetsDay);
            mockIDataReader.SetupSequence(m => m.Read())
              .Returns(true)
              .Returns(false);

            var mockObjectDB = new Mock<IObjectDb>();

            mockObjectDB.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>()))
                .Returns(Task.FromResult(mockIDataReader.Object));

            ObjectDbFactory.SwitchToTestingMode();
            ObjectDbFactory.Mock((x, y) => mockObjectDB.Object);

            var service = new CommonService(_memoryCache);


            ///Action

            var result = await service.GetBalanceUpAsync().ConfigureAwait(false);

            ///Assert
            Assert.Equal(result.BalanceUpDay, balanceupInfo.BalanceUpDay);
            Assert.Equal(result.MoveBetsDay, balanceupInfo.MoveBetsDay);
        }

    }
}
