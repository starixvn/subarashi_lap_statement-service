using Fanex.Athena.ExtService.Common.Models.DTO;
using Fanex.Athena.ExtService.Common.Services;
using Fanex.Data;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Fanex.Athena.ExtService.UnitTest.Common.Helpers;
using Xunit;

namespace Fanex.Athena.ExtService.Common.UnitTests.Services
{
    public class CommonServiceTest
    {
        private const string BalanceUpCacheKey = "EXT_LAP_CACHE_BALANCEUP";
        private const string LicenseeExtInfoCacheKey = "__LicenseeExtInfoCacheKey_";

        private readonly Mock<ICustomDistributedCache> _mockCache;

        private readonly Mock<IOptions<AppSettings>> _mockAppSettings;

        public CommonServiceTest()
        {
            _mockCache = new Mock<ICustomDistributedCache>();
            _mockAppSettings = new Mock<IOptions<AppSettings>>();
            DbSettingProviderManager.StartTestingSession().Run();
        }

        [Theory]
        [InlineData("Any", true)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void IsShowIp_WhenCall_ReturnCorrectValue(string siteName, bool expected)
        {
            //Arrange
            var service = new CommonService(_mockCache.Object, _mockAppSettings.Object);

            //Action
            var result = service.IsShowIp(siteName);

            //Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetBalanceUpAsync_WhenCallWithNoDataInCache_GetResulFromDB()
        {
            //Arrange
            var balanceupInfo = new BalanceUp
            {
                BalanceUpDay = DateTime.Now.AddHours(3),
                MoveBetsDay = DateTime.Now.AddDays(-2)
            };
            SetupBalanceUpDatabaseData(balanceupInfo);
            _mockCache.SetReturnsDefault(Task.FromResult((byte[])null));

            var service = new CommonService(_mockCache.Object, _mockAppSettings.Object);

            //Action
            var result = await service.GetBalanceUpAsync().ConfigureAwait(false);

            //Assert
            Assert.Equal(result.BalanceUpDay, balanceupInfo.BalanceUpDay);
            Assert.Equal(result.MoveBetsDay, balanceupInfo.MoveBetsDay);
        }

        [Fact]
        public async Task GetBalanceUpAsync_ResulFromDB_SetCache()
        {
            //Arrange
            SetupBalanceUpDatabaseData(new BalanceUp());
            _mockCache.SetReturnsDefault(Task.FromResult((byte[])null));

            var service = new CommonService(_mockCache.Object, _mockAppSettings.Object);

            //Action
            var result = await service.GetBalanceUpAsync().ConfigureAwait(false);

            //Assert
            _mockCache.Verify(c => c.SetAsync(BalanceUpCacheKey,
                It.IsAny<BalanceUp>(),
                It.Is<DistributedCacheEntryOptions>(o => o.AbsoluteExpiration != null),
                It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GetBalanceUpAsync_WhenCall_ReturnDataFromCache()
        {
            //Arrange
            var balanceupInfo = new BalanceUp
            {
                BalanceUpDay = DateTime.Now,
                MoveBetsDay = DateTime.Now.AddMonths(-1)
            };

            _mockCache.Setup(c => c.GetAsync<BalanceUp>(BalanceUpCacheKey, It.IsAny<CancellationToken>())).ReturnsAsync(balanceupInfo);

            var service = new CommonService(_mockCache.Object, _mockAppSettings.Object);

            //Action
            var result = await service.GetBalanceUpAsync().ConfigureAwait(false);

            //Assert
            Assert.Equal(balanceupInfo.MoveBetsDay, result.MoveBetsDay);
            Assert.Equal(balanceupInfo.BalanceUpDay, result.BalanceUpDay);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public async Task GetLicenseeInfoAsync_WhenCallWithoutdataInCache_ReturnDataFromDB(int custId)
        {
            //Arrange
            const string SiteName = "12Bet";
            const int CurrencyId = 1;
            const int AgentId = 2;
            const string CurrencyName = "USD";
            const bool HasComm = true;

            SetupLicenseeInfoDatabaseData(CurrencyId, CurrencyName, AgentId, HasComm);
            _mockCache.SetReturnsDefault(Task.FromResult((byte[])null));

            var service = new CommonService(_mockCache.Object, _mockAppSettings.Object);

            ///Action

            var result = await service.GetLicenseeInfoAsync(SiteName, custId).ConfigureAwait(false);

            ///Assert
            Assert.Equal(SiteName, result.SiteName);
            Assert.Equal(HasComm, result.IsAllowedCustomerComm);
            Assert.Equal(CurrencyId, result.Currencies[0].CurrencyId);
            Assert.Equal(CurrencyName, result.Currencies[0].CurrencyName);
            Assert.Equal(AgentId, result.Currencies[0].AgentId);

            var objectDb = ObjectDbFactory.CreateInstance("mock");
            var mockObjectDB = Mock.Get(objectDb);
            mockObjectDB.Verify(x => x.SetParameterValue("siteName", SiteName), Times.Once);
            mockObjectDB.Verify(x => x.SetParameterValue("custId", custId), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public async Task GetLicenseeInfoAsync_DataFromDB_SetCache(int custId)
        {
            //Arrange
            const string SiteName = "12Bet-test";
            const int CurrencyId = 10;
            const int AgentId = 20;
            const string CurrencyName = "EUR";
            const bool HasComm = false;

            SetupLicenseeInfoDatabaseData(CurrencyId, CurrencyName, AgentId, HasComm);

            var realCacheKey = $"{LicenseeExtInfoCacheKey}{SiteName}_{custId}";
            _mockCache.SetReturnsDefault(Task.FromResult<LicenseeInfo>(null));

            var service = new CommonService(_mockCache.Object, _mockAppSettings.Object);
            ///Action

            var result = await service.GetLicenseeInfoAsync(SiteName, custId).ConfigureAwait(false);

            ///Assert
            _mockCache.Verify(c => c.SetAsync(realCacheKey, It.IsAny<LicenseeInfo>(), It.Is<DistributedCacheEntryOptions>(o =>
                (custId > 0 && o.SlidingExpiration != null) ||
                (custId == 0 && o.AbsoluteExpiration != null)), It.IsAny<CancellationToken>()));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public async Task GetLicenseeInfoAsync_WhenCall_ReturnDataFromCache(int custId)
        {
            //Arrange
            const string SiteName = "12Bet";
            const int CurrencyId = 1;
            const int AgentId = 2;
            const string CurrencyName = "USD";
            const bool HasComm = true;
            var realCacheKey = $"{LicenseeExtInfoCacheKey}{SiteName}_{custId}";
            var licenseeInfo = new LicenseeInfo
            {
                Currencies = new[] { new CurrencyInfo { CurrencyId = CurrencyId, CurrencyName = CurrencyName, AgentId = AgentId } },
                SiteName = SiteName,
                IsAllowedCustomerComm = HasComm
            };

            _mockCache.Setup(c => c.GetAsync<LicenseeInfo>(realCacheKey, It.IsAny<CancellationToken>()))
                .ReturnsAsync(licenseeInfo);

            var service = new CommonService(_mockCache.Object, _mockAppSettings.Object);

            ///Action

            var result = await service.GetLicenseeInfoAsync(SiteName, custId).ConfigureAwait(false);

            ///Assert
            Assert.Equal(SiteName, result.SiteName);
            Assert.Equal(HasComm, result.IsAllowedCustomerComm);
            Assert.Equal(CurrencyId, result.Currencies[0].CurrencyId);
            Assert.Equal(CurrencyName, result.Currencies[0].CurrencyName);
            Assert.Equal(AgentId, result.Currencies[0].AgentId);
        }

        [Theory]
        [InlineData("UATTW", "UAT")]
        [InlineData("PROD", "Production")]
        [InlineData("LOCAL", "Production")]
        [InlineData("SIT", "Production")]
        public async Task IsEnableSingleWallet_WhenCall_CorrectlySPParams(string settingEnv, string spParamEnv)
        {
            //Arrange
            const string SiteName = "12Bet";
            var appSetting = FakeHelper.Create<AppSettings>();
            appSetting.Environment = settingEnv;
            _mockAppSettings.Setup(_ => _.Value).Returns(appSetting);

            var mockObjectDb = new Mock<IObjectDb>();
            ObjectDbFactory.SwitchToTestingMode();
            ObjectDbFactory.Mock((_, _) => mockObjectDb.Object);

            var service = new CommonService(_mockCache.Object, _mockAppSettings.Object);

            //Action
            _ = await service.IsEnableSingleWallet(SiteName);

            //Assert
            mockObjectDb.Verify(_ => _.SetParameterValue("SiteName", SiteName), Times.Once);
            mockObjectDb.Verify(_ => _.SetParameterValue("Environment", spParamEnv), Times.Once);
            mockObjectDb.Verify(_ => _.SetParameterValue("KeyName", "EnableSingleWallet"), Times.Once);
            mockObjectDb.Verify(_ => _.SetParameterValue("KeyValue", null), Times.Once);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task IsEnableSingleWallet_WhenCall_CorrectlyResultFromOutputParam(bool expectedResult)
        {
            //Arrange
            const string SiteName = "12Bet";
            var appSetting = FakeHelper.Create<AppSettings>();
            _mockAppSettings.Setup(_ => _.Value).Returns(appSetting);

            var mockObjectDb = new Mock<IObjectDb>();
            mockObjectDb.Setup(_ => _.GetParameterValue("KeyValue")).Returns(expectedResult);
            ObjectDbFactory.SwitchToTestingMode();
            ObjectDbFactory.Mock((_, _) => mockObjectDb.Object);

            var service = new CommonService(_mockCache.Object, _mockAppSettings.Object);

            //Action
            var result = await service.IsEnableSingleWallet(SiteName);

            //Assert
            Assert.Equal(expectedResult, result);
        }

        private void SetupBalanceUpDatabaseData(BalanceUp balanceup)
        {
            var mockIDataReader = new Mock<IDataReader>();
            mockIDataReader.Setup(m => m["balanceupday"]).Returns(balanceup.BalanceUpDay);
            mockIDataReader.Setup(m => m["movebetsday"]).Returns(balanceup.MoveBetsDay);
            mockIDataReader.SetupSequence(m => m.Read()).Returns(true).Returns(false);
            MockObjectDbFactory(mockIDataReader.Object);
        }

        private void SetupLicenseeInfoDatabaseData(int currencyId, string currencyName, int agentId, bool hasComm)
        {
            var mockIDataReader = new Mock<IDataReader>();
            mockIDataReader.Setup(m => m["CurrencyId"]).Returns(currencyId);
            mockIDataReader.Setup(m => m["AgentId"]).Returns(agentId);
            mockIDataReader.Setup(m => m["CurrencyName"]).Returns(currencyName);
            mockIDataReader.Setup(m => m["IsAllowedCustomerComm"]).Returns(hasComm);
            mockIDataReader.Setup(x => x.NextResult()).Returns(true);
            mockIDataReader.SetupSequence(m => m.Read())
              .Returns(true)
              .Returns(false)
              .Returns(true)
              .Returns(false);

            MockObjectDbFactory(mockIDataReader.Object);
        }

        private static void MockObjectDbFactory(IDataReader reader)
        {
            var mockObjectDB = new Mock<IObjectDb>();
            mockObjectDB.Setup(x =>
            x.ExecuteReaderAsync(It.IsAny<object>(),
            It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(reader);

            ObjectDbFactory.SwitchToTestingMode();
            ObjectDbFactory.Mock((x, y) => mockObjectDB.Object);
        }
    }
}