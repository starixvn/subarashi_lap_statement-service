using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Fanex.Athena.ExtService.BetList.Controllers;
using Fanex.Athena.ExtService.BetList.Models.MetaModel;
using Fanex.Athena.ExtService.BetList.Models.ViewModel;
using Fanex.Athena.ExtService.Common.Models.DTO;
using Fanex.Athena.ExtService.Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Fanex.Athena.ExtService.BetList.UnitTests.Controllers
{
    public class BetListControllerTest : BaseControllerTest
    {
        private readonly Mock<ICommonService> _commonService;
        private readonly Mock<IBetListApiClient> _betListApiClient;
        public BetListControllerTest() : base()
        {
            _commonService = new Mock<ICommonService>();
            _commonService.Setup(x => x.GetLicenseeInfoAsync(SiteName, It.IsAny<int>()))
                .Returns(Task.FromResult(new LicenseeInfo
                {
                    SiteName = SiteName,
                    Currencies = new CurrencyInfo[] { new CurrencyInfo {
                        CurrencyId =1,
                        CurrencyName="UUS",
                        AgentId=100
                    } },
                    IsAllowedCustomerComm = true

                }));

            _commonService.Setup(x => x.GetBalanceUpAsync()).Returns(Task.FromResult(
                new BalanceUp { BalanceUpDay = DateTime.Now, MoveBetsDay = DateTime.Now }
                ));

            _betListApiClient = new Mock<IBetListApiClient>();

        }

        [Fact]
        public async Task RefNo_WhenExecute_AlwayAssignCommonViewModelValue()
        {
            //Arrange
            const string UserName = "Any";
            const string Title = "Bet List By RefNo";
            var metalModel = new RefNoBetListMetaModel { UserName = UserName };

            IActionResult expectedResult = null;
            var licenseeInfo = await _commonService.Object.GetLicenseeInfoAsync(SiteName, 0);
            var balanceUp = await _commonService.Object.GetBalanceUpAsync();
            //Action
            using (var controller = new BetListController(_commonService.Object, _betListApiClient.Object))
            {
                controller.ControllerContext = _controllerContext.Object;
                expectedResult = await controller.RefNo(metalModel).ConfigureAwait(false);
            }

            //Assert
            var viewResult = Assert.IsType<ViewResult>(expectedResult);
            var viewModel = Assert.IsType<RefNoViewModel>(viewResult.Model);
            var title = Assert.IsAssignableFrom<string>(viewResult.ViewData["Title"]);
            Assert.Equal(viewModel.Info, licenseeInfo);
            Assert.Equal(viewModel.BalanceUp, balanceUp);
            Assert.Equal(viewModel.UserName, UserName);
            Assert.Equal(viewModel.BaseCurrency, metalModel.BaseCurrency);
            Assert.Equal(viewModel.Lang, metalModel.Lang);
            Assert.Equal(viewModel.AccessToken, AccessToken);
            Assert.Equal(title, Title);

        }
    }
}
