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
        private const int CacheTimeout120Seconds = 120;
        private const int CacheTimeout300Seconds = 300;
        private const int CacheTimeOut30Seconds = 30;
        private const string BalanceUpCacheKey = "EXT_LAP_CACHE_BALANCEUP";
        private const string LicenseeExtInfoCacheKey = "__LicenseeExtInfoCacheKey_";

        private readonly IMemoryCache _memoryCache;
        public CommonServiceTest()
        {
            DbSettingProviderManager
             .StartTestingSession()
             .Run();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        [Fact]
        public async Task GetBalanceUpAsync_WhenCallWithNoDataInCache_GetResulFromDBAndSetCache()
        {
            //Arrange
            var balanceupInfo = new BalanceUp
            {
                BalanceUpDay = DateTime.Now,
                MoveBetsDay = DateTime.Now
            };

            var mockIDataReader = new Mock<IDataReader>();

            _memoryCache.Remove(BalanceUpCacheKey);

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
            Assert.True(_memoryCache.TryGetValue(BalanceUpCacheKey, out BalanceUp cacheResult));
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
            _memoryCache.Set(BalanceUpCacheKey, balanceupInfo);

            var service = new CommonService(_memoryCache);
            ///Action
            var result = await service.GetBalanceUpAsync().ConfigureAwait(false);

            ///Assert
            Assert.Equal(result, balanceupInfo);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public async Task GetLicenseeInfoAsync_WhenCallWithoutdataInCache_ReturnDataFromDBAndSetCache(int custId)
        {
            //Arrange
            const string SiteName = "12Bet";
            const int CurrencyId = 1;
            const int AgentId = 2;
            const string CurrencyName = "USD";
            const bool HasComm = true;
            var mockIDataReader = new Mock<IDataReader>();
            var realCacheKey = $"{LicenseeExtInfoCacheKey}{SiteName}_{custId}";
            var realcacheKeyBySite = $"{LicenseeExtInfoCacheKey}{SiteName}_{0}";
            _memoryCache.Remove(realCacheKey);
            _memoryCache.Remove(realcacheKeyBySite);
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

            var result = await service.GetLicenseeInfoAsync(SiteName, custId).ConfigureAwait(false);

            ///Assert
            Assert.Equal(result.SiteName, SiteName);
            Assert.Equal(result.IsAllowedCustomerComm, HasComm);
            Assert.Equal(result.Currencies[0].CurrencyId, CurrencyId);
            Assert.Equal(result.Currencies[0].CurrencyName, CurrencyName);
            Assert.Equal(result.Currencies[0].AgentId, AgentId);
            mockObjectDB.Verify(x => x.SetParameterValue("siteName", SiteName), Times.Once);
            mockObjectDB.Verify(x => x.SetParameterValue("custId", custId), Times.Once);
            Assert.True(_memoryCache.TryGetValue(realCacheKey, out LicenseeInfo cacheResult));


        }

        [Theory]
        [InlineData(0, true)]
        [InlineData(1, false)]
        [InlineData(1, true)]
        public async Task GetLicenseeInfoAsync_WhenCall_ReturnDataFromCache(int custId, bool isCachedBySiteName)
        {
            //Arrange
            const string SiteName = "12Bet";
            const int CurrencyId = 1;
            const int AgentId = 2;
            const string CurrencyName = "USD";
            const bool HasComm = true;
            var realCacheKey = $"{LicenseeExtInfoCacheKey}{SiteName}_{(isCachedBySiteName ? 0 : custId)}";
            var cacheResult = new LicenseeInfo
            {

                Currencies = new[] { new CurrencyInfo { CurrencyId = CurrencyId, CurrencyName = CurrencyName, AgentId = AgentId } },
                SiteName = SiteName,
                IsAllowedCustomerComm = HasComm
            };

            _memoryCache.Set(realCacheKey, cacheResult);

            var service = new CommonService(_memoryCache);

            ///Action

            var result = await service.GetLicenseeInfoAsync(SiteName, custId).ConfigureAwait(false);

            ///Assert
            Assert.Equal(result.SiteName, SiteName);
            Assert.Equal(result.IsAllowedCustomerComm, HasComm);
            Assert.Equal(result.Currencies[0].CurrencyId, CurrencyId);
            Assert.Equal(result.Currencies[0].CurrencyName, CurrencyName);
            Assert.Equal(result.Currencies[0].AgentId, AgentId);
        }


        public void Dispose()
        {
            _memoryCache?.Dispose();
        }
    }
}
