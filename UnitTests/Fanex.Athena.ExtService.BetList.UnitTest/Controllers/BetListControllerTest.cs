using Fanex.Athena.ExtService.UnitTest.Common.Helpers;

namespace Fanex.Athena.ExtService.BetList.UnitTests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.ExtService.BetList.Controllers;
    using Fanex.Athena.ExtService.BetList.Models.DTO;
    using Fanex.Athena.ExtService.BetList.Models.MetaModel;
    using Fanex.Athena.ExtService.BetList.Models.ViewModel;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Xunit;

    public class BetListControllerTest : BaseControllerTest
    {
        [Fact]
        public async Task RefNo_WhenExecute_AlwaysAssignCommonViewModelValue()
        {
            // Arrange
            const string UserName = "Any";
            const string Title = "Bet List By RefNo";
            
            var metalModel = new RefNoBetListMetaModel { UserName = UserName };

            IActionResult result = null;
            var licenseeInfo = await _commonService.Object.GetLicenseeInfoAsync(SiteName, 0);

            // Action
            using (var controller = new BetListController(_commonService.Object, _betListApiClient.Object, _browserService.Object))
            {
                controller.ControllerContext = _controllerContext.Object;
                result = await controller.RefNo(metalModel).ConfigureAwait(false);
            }

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<RefNoViewModel>(viewResult.Model);
            var title = Assert.IsAssignableFrom<string>(viewResult.ViewData["Title"]);
            Assert.Equal(licenseeInfo, viewModel.Info);
            Assert.Equal(UserName, viewModel.UserName);
            Assert.Equal(metalModel.BaseCurrency, viewModel.BaseCurrency);
            Assert.Equal(metalModel.Lang, viewModel.Lang);
            Assert.Equal(AccessToken, viewModel.AccessToken);
            Assert.Equal(Title, title);
        }

        [Fact]
        public async Task RefNo_InValidMetalModel_ReturnEmptyData()
        {
            // Arrange
            const string UserName = "Any";
            var metalModel = new RefNoBetListMetaModel { UserName = UserName };

            IActionResult expectedResult = null;

            // Action
            using (var controller = new BetListController(_commonService.Object, _betListApiClient.Object, _browserService.Object))
            {
                controller.ControllerContext = _controllerContext.Object;
                expectedResult = await controller.RefNo(metalModel).ConfigureAwait(false);
            }

            // Assert
            var viewResult = Assert.IsType<ViewResult>(expectedResult);
            var viewModel = Assert.IsType<RefNoViewModel>(viewResult.Model);
            Assert.Empty(viewModel.Data);
        }

        [Fact]
        public async Task RefNo_ValidMetalModel_ReturnCorrectDataList()
        {
            // Arrange
            const string UserName = "Any";
            var metalModel = new RefNoBetListMetaModel { UserName = UserName, RefNo = 100 };
            var ticketText = FakeHelper.Create<TicketText>();
            var apiResult = FakeHelper.Create<RefNoDataResult>(result => result.Data, 
                                                               new List<TicketText> { ticketText });

            _betListApiClient.Setup(x => x.PostAsync<RefNoDataResult>(
                "/BettingTicketCenter/GetByRefno",
                It.IsAny<RefNoBetListMetaModel>(),
                It.IsAny<string>()
               )).Returns(Task.FromResult(apiResult));
            IActionResult expectedResult = null;

            // Action
            using (var controller = new BetListController(_commonService.Object, _betListApiClient.Object, _browserService.Object))
            {
                controller.ControllerContext = _controllerContext.Object;
                expectedResult = await controller.RefNo(metalModel).ConfigureAwait(false);
            }

            // Assert
            var viewResult = Assert.IsType<ViewResult>(expectedResult);
            var viewModel = Assert.IsType<RefNoViewModel>(viewResult.Model);
            Assert.NotNull(viewModel.Data);
            Assert.Equal(apiResult.Data, viewModel.Data);
        }

        [Fact]
        public async Task TxID_WhenExecute_AlwaysAssignCommonViewModelValue()
        {
            // Arrange
            const string UserName = "Any";
            const string Title = "Bet List By TxID";
            var metalModel = new TxIDBetListMetaModel { UserName = UserName };

            IActionResult result = null;
            var licenseeInfo = await _commonService.Object.GetLicenseeInfoAsync(SiteName, 0);

            // Action
            using (var controller = new BetListController(_commonService.Object, _betListApiClient.Object, _browserService.Object))
            {
                controller.ControllerContext = _controllerContext.Object;
                result = await controller.TxID(metalModel).ConfigureAwait(false);
            }

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<TxIDViewModel>(viewResult.Model);
            var title = Assert.IsAssignableFrom<string>(viewResult.ViewData["Title"]);
            Assert.Equal(licenseeInfo, viewModel.Info);
            Assert.Equal(UserName, viewModel.UserName);
            Assert.Equal(metalModel.BaseCurrency, viewModel.BaseCurrency);
            Assert.Equal(metalModel.Lang, viewModel.Lang);
            Assert.Equal(AccessToken, viewModel.AccessToken);
            Assert.Equal(Title, title);
        }

        [Fact]
        public async Task TxID_InValidMetalModel_ReturnEmptyData()
        {
            // Arrange
            const string UserName = "Any";
            var metalModel = new TxIDBetListMetaModel { UserName = UserName };

            IActionResult expectedResult = null;

            // Action
            using (var controller = new BetListController(_commonService.Object, _betListApiClient.Object, _browserService.Object))
            {
                controller.ControllerContext = _controllerContext.Object;
                expectedResult = await controller.TxID(metalModel).ConfigureAwait(false);
            }

            // Assert
            var viewResult = Assert.IsType<ViewResult>(expectedResult);
            var viewModel = Assert.IsType<TxIDViewModel>(viewResult.Model);
            Assert.Empty(viewModel.Data);
        }

        [Fact]
        public async Task TxID_ValidMetalModel_ReturnCorrectDataList()
        {
            // Arrange
            const string userName = "Any";
            var metalModel = new TxIDBetListMetaModel { UserName = userName, TxID = "100" };
            var ticketText = FakeHelper.Create<TicketText>();
            var apiResult = FakeHelper.Create<TxIDDataResult>(result => result.Data, 
                                                              new List<TicketText> { ticketText });

            _betListApiClient.Setup(x => x.PostAsync<TxIDDataResult>(
                "/BettingTicketCenter/GetByTxID",
                It.IsAny<TxIDBetListMetaModel>(),
                It.IsAny<string>()
               )).Returns(Task.FromResult(apiResult));
            IActionResult expectedResult = null;

            // Action
            using (var controller = new BetListController(_commonService.Object, _betListApiClient.Object, _browserService.Object))
            {
                controller.ControllerContext = _controllerContext.Object;
                expectedResult = await controller.TxID(metalModel).ConfigureAwait(false);
            }

            // Assert
            var viewResult = Assert.IsType<ViewResult>(expectedResult);
            var viewModel = Assert.IsType<TxIDViewModel>(viewResult.Model);
            Assert.NotNull(viewModel.Data);
            Assert.Equal(apiResult.Data, viewModel.Data);
        }

        [Fact]
        public async Task ByGroup_WhenExecute_AlwaysAssignCommonViewModelValue()
        {
            // Arrange
            const string Title = "Bet List By Group";
            var metalModel = new BetListByGroupMetaModel();

            IActionResult result = null;
            var licenseeInfo = await _commonService.Object.GetLicenseeInfoAsync(SiteName, 0);

            // Action
            using (var controller = new BetListController(_commonService.Object, _betListApiClient.Object, _browserService.Object))
            {
                controller.ControllerContext = _controllerContext.Object;
                result = await controller.ByGroup(metalModel).ConfigureAwait(false);
            }

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<BetListByProductViewModel>(viewResult.Model);
            var title = Assert.IsAssignableFrom<string>(viewResult.ViewData["Title"]);
            Assert.Equal(licenseeInfo, viewModel.Info);
            Assert.Equal(metalModel.BaseCurrency, viewModel.BaseCurrency);
            Assert.Equal(metalModel.Lang, viewModel.Lang);
            Assert.Equal(AccessToken, viewModel.AccessToken);

            Assert.Equal(Title, title);
        }

        [Fact]
        public async Task ByGroup_InValidMetalModel_ReturnEmptyData()
        {
            // Arrange
            var metalModel = new BetListByGroupMetaModel { };

            IActionResult expectedResult = null;

            // Action
            using (var controller = new BetListController(_commonService.Object, _betListApiClient.Object, _browserService.Object))
            {
                controller.ControllerContext = _controllerContext.Object;
                expectedResult = await controller.ByGroup(metalModel).ConfigureAwait(false);
            }

            // Assert
            var viewResult = Assert.IsType<ViewResult>(expectedResult);
            var viewModel = Assert.IsType<BetListByProductViewModel>(viewResult.Model);
            Assert.Empty(viewModel.Data);
        }

        [Fact]
        public async Task ByGroup_ValidMetalModel_ReturnCorrectDataList()
        {
            // Arrange
            var metalModel = new BetListByGroupMetaModel
            {
                ProductCode = "SB",
                PageIndex = 1,
                PageSize = 500,
                ModeView = 1,
                CustId = 100,
                WinlostDate = DateTime.Now
            };
            var ticketText = FakeHelper.Create<TicketText>();
            var apiResult = FakeHelper.Create<RefNoDataResult>(result => result.Data, 
                                                               new List<TicketText> { ticketText });

            _betListApiClient.Setup(x => x.PostAsync<RefNoDataResult>(
                                                                      "/BetListByProduct/Index",
                                                                      It.IsAny<BetListByGroupMetaModel>(),
                                                                      It.IsAny<string>()
                                                                     )).Returns(Task.FromResult(apiResult));

            IActionResult expectedResult = null;

            // Action
            using (var controller = new BetListController(_commonService.Object, _betListApiClient.Object, _browserService.Object))
            {
                controller.ControllerContext = _controllerContext.Object;
                expectedResult = await controller.ByGroup(metalModel).ConfigureAwait(false);
            }

            // Assert
            var viewResult = Assert.IsType<ViewResult>(expectedResult);
            var viewModel = Assert.IsType<BetListByProductViewModel>(viewResult.Model);
            Assert.NotNull(viewModel.Data);
            Assert.Equal(apiResult.Data, viewModel.Data);
        }

        [Fact]
        public async Task Running_ValidMetalModel_ReturnCorrectDataList()
        {
            // Arrange
            var metalModel = new RunningBetListMetaModel
            {
                PageIndex = 1,
                PageSize = 500,
                CustId = 100
            };
            var ticketText = FakeHelper.Create<TicketText>();
            var apiResult = FakeHelper.Create<RefNoDataResult>(result => result.Data, new List<TicketText> { ticketText });

            _betListApiClient.Setup(x => x.PostAsync<RefNoDataResult>(
                                                                      "/PlayerAdminBetList/GetBetList",
                                                                      It.IsAny<RunningBetListMetaModel>(),
                                                                      It.IsAny<string>()
                                                                     )).Returns(Task.FromResult(apiResult));

            IActionResult expectedResult = null;

            // Action
            using (var controller = new BetListController(_commonService.Object, _betListApiClient.Object, _browserService.Object))
            {
                controller.ControllerContext = _controllerContext.Object;
                expectedResult = await controller.Running(metalModel).ConfigureAwait(false);
            }

            // Assert
            var viewResult = Assert.IsType<ViewResult>(expectedResult);
            var viewModel = Assert.IsType<RunningBetListByViewModel>(viewResult.Model);
            Assert.NotNull(viewModel.Data);
            Assert.Equal(apiResult.Data, viewModel.Data);
        }

        [Fact]
        public async Task Running_InValidMetalModel_ReturnEmptyData()
        {
            // Arrange
            var metalModel = new RunningBetListMetaModel { };
            IActionResult expectedResult = null;

            // Action
            using (var controller = new BetListController(_commonService.Object, _betListApiClient.Object, _browserService.Object))
            {
                controller.ControllerContext = _controllerContext.Object;
                expectedResult = await controller.Running(metalModel).ConfigureAwait(false);
            }

            // Assert
            var viewResult = Assert.IsType<ViewResult>(expectedResult);
            var viewModel = Assert.IsType<RunningBetListByViewModel>(viewResult.Model);
            Assert.Empty(viewModel.Data);
        }

        [Fact]
        public async Task Running_WhenExecute_AlwaysAssignCommonViewModelValue()
        {
            // Arrange
            const string Title = "Running Bet List";
            var metalModel = new RunningBetListMetaModel
            {
                PageIndex = 2,
                PageSize = 100,
                CustId = 200,
                Lang = "Lang"
            };

            _betListApiClient.Setup(x => x.PostAsync<RefNoDataResult>(
                "/PlayerAdminBetList/GetBetList",
                It.IsAny<RunningBetListMetaModel>(),
                It.IsAny<string>()
                )).Returns(Task.FromResult(new RefNoDataResult()));

            IActionResult result = null;
            var licenseeInfo = await _commonService.Object.GetLicenseeInfoAsync(SiteName, 0);

            // Action
            using (var controller = new BetListController(_commonService.Object, _betListApiClient.Object, _browserService.Object))
            {
                controller.ControllerContext = _controllerContext.Object;
                result = await controller.Running(metalModel).ConfigureAwait(false);
            }

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<RunningBetListByViewModel>(viewResult.Model);
            var title = Assert.IsAssignableFrom<string>(viewResult.ViewData["Title"]);
            Assert.Equal(licenseeInfo, viewModel.Info);
            Assert.Equal(string.Empty, viewModel.BaseCurrency);
            Assert.Equal(metalModel.Lang, viewModel.Lang);
            Assert.Equal(AccessToken, viewModel.AccessToken);
            Assert.Equal(Title, title);
        }
    }
}
