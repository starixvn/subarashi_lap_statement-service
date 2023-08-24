using System.Collections.Generic;
using System.Threading.Tasks;
using Fanex.Athena.ExtService.BetList.Controllers;
using Fanex.Athena.ExtService.BetList.Models.DTO;
using Fanex.Athena.ExtService.BetList.Models.MetaModel;
using Fanex.Athena.ExtService.BetList.Models.ViewModel;
using Fanex.Athena.ExtService.UnitTest.Common.Helpers;
using Microsoft.AspNetCore.Mvc;

using Moq;

using Xunit;

namespace Fanex.Athena.ExtService.BetList.UnitTests.Controllers
{
    public class CashOutBetListControllerTest : BaseControllerTest
    {
        [Fact]
        public async Task Detail_WhenCall_ReturnCorrectViewModelAsync()
        {
            //Arrange
            const string Title = "Cash Out BetList";
            var metalModel = FakeHelper.Create<CashOutBetListDetailMetaModel>();
            metalModel.Lang = "en-US";

            var cashOutTicket = FakeHelper.Create<CashOutTicket>();
            var cashOutTicketText = FakeHelper.Create<CashOutTicketText>();

            var fakeResult = FakeHelper.Create(new List<FakeProperty<CashOutBetListDetailViewModel>>
            {
                new (result => result.CashOutTicketList, new List<CashOutTicket> { cashOutTicket }),
                new (result => result.CashOutTicketTextList, new List<CashOutTicketText>{ cashOutTicketText })
            });

            IActionResult result = null;
            _betListApiClient.Setup(x => x.PostAsync<CashOutBetListDetailViewModel>(
                "/CashOutBetList/Detail", It.IsAny<CashOutBetListDetailMetaModel>(),
                It.IsAny<string>()))
                .Returns(Task.FromResult(fakeResult));

            var licenseeInfo = await _commonService.Object.GetLicenseeInfoAsync(SiteName, 0);

            //Action
            using (var controller = new CashOutBetListController(
                _commonService.Object, _betListApiClient.Object, _browserService.Object))
            {
                controller.ControllerContext = _controllerContext.Object;
                result = await controller.Detail(metalModel).ConfigureAwait(false);
            }

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<CashOutBetListDetailViewModel>(viewResult.Model);
            var title = Assert.IsAssignableFrom<string>(viewResult.ViewData["Title"]);

            Assert.NotNull(viewModel);
            Assert.Equal(fakeResult.OriginalTicketText, viewModel.OriginalTicketText);
            Assert.Equal(fakeResult.CashOutTicketList, viewModel.CashOutTicketList);
            Assert.Equal(fakeResult.CashOutTicketTextList, viewModel.CashOutTicketTextList);
            Assert.Equal(fakeResult.IsCashOut, viewModel.IsCashOut);
            Assert.Equal(fakeResult.IsHighlightOriginalTicket, viewModel.IsHighlightOriginalTicket);
            Assert.Equal(licenseeInfo, viewModel.Info);
            Assert.Equal(metalModel.BaseCurrency, viewModel.BaseCurrency);
            Assert.Equal(metalModel.Lang, viewModel.Lang);
            Assert.Equal(AccessToken, viewModel.AccessToken);
            Assert.Equal(Title, title);
        }

        [Fact]
        public async Task NewDetail_WhenCall_ReturnCorrectViewModelAsync()
        {
            //Arrange
            const string Title = "Cash Out History";
            var metalModel = FakeHelper.Create<NewCashOutBetListDetailMetaModel>();
            metalModel.Lang = "en-US";
            metalModel.Currency = "UUS";

            var fakeResult = new NewCashOutBetListDetailViewModel();
            IActionResult result = null;
            _betListApiClient.Setup(x => x.PostAsync<NewCashOutBetListDetailViewModel>(
                "/CashOutBetList/History", It.IsAny<NewCashOutBetListDetailMetaModel>(),
                It.IsAny<string>()))
                .Returns(Task.FromResult(fakeResult));

            var licenseeInfo = await _commonService.Object.GetLicenseeInfoAsync(SiteName, 0);

            //Action
            using (var controller = new CashOutBetListController(
                _commonService.Object, _betListApiClient.Object, _browserService.Object))
            {
                controller.ControllerContext = _controllerContext.Object;
                result = await controller.NewDetail(metalModel).ConfigureAwait(false);
            }

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<NewCashOutBetListDetailViewModel>(viewResult.Model);
            var title = Assert.IsAssignableFrom<string>(viewResult.ViewData["Title"]);

            Assert.NotNull(viewModel);
            Assert.Equal(fakeResult.CashoutExtraInfo, viewModel.CashoutExtraInfo);
            Assert.Equal(fakeResult.Currency, viewModel.Currency);
            Assert.Equal(fakeResult.DetailHistory, viewModel.DetailHistory);
            Assert.Equal(fakeResult.Rate, viewModel.Rate);
            Assert.Equal(fakeResult.TransId, viewModel.TransId);
            Assert.Equal(licenseeInfo, viewModel.Info);
            Assert.Equal(metalModel.BaseCurrency, viewModel.BaseCurrency);
            Assert.Equal(metalModel.Lang, viewModel.Lang);
            Assert.Equal(AccessToken, viewModel.AccessToken);
            Assert.Equal(Title, title);
        }

    }
}
