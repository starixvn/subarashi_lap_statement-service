using System;
using System.Threading.Tasks;

using Fanex.Athena.ExtService.BetList.Controllers;
using Fanex.Athena.ExtService.BetList.Models.DataDomain;
using Fanex.Athena.ExtService.BetList.Models.MetaModel;
using Fanex.Athena.ExtService.BetList.Models.ViewModel;
using Fanex.Athena.ExtService.BetList.Services;
using Fanex.Athena.ExtService.Common.Services;
using Fanex.Athena.ExtService.UnitTest.Common.Helpers;
using Microsoft.AspNetCore.Mvc;

using Moq;

using Xunit;

namespace Fanex.Athena.ExtService.BetList.UnitTests.Controllers
{
    public class StatementControllerTest : BaseControllerTest
    {
        [Theory]
        [InlineData(0)]
        [InlineData(100)]
        public async Task Summary_WhenExecute_AlwaysAssignCommonViewModelValue(int custId)
        {
            //Arrange
            var metalModel = new SummaryStatementMetaModel
            {
                CustId = custId,
                Lang = "en-US",
                BaseCurrency = "USD"
            };
            var fakeData = FakeHelper.Create<SummaryStatementViewModel>();
            IActionResult expectedResult = null;
            var fakeService = new Mock<ISummaryStatementService>();
            fakeService.Setup(x => x.QueryAsync(It.IsAny<SummaryStatementMetaModel>()))
            .Returns(Task.FromResult(fakeData));
            //Action
            var controller = new StatementController(_commonService.Object);
            controller.ControllerContext = _controllerContext.Object;
            expectedResult = await controller.Summary(fakeService.Object, metalModel).ConfigureAwait(false);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(expectedResult);
            var viewModel = Assert.IsType<SummaryStatementViewModel>(viewResult.Model);
            Assert.Equal(metalModel.BaseCurrency, viewModel.BaseCurrency);
            Assert.Equal(metalModel.Lang, viewModel.Lang);
            Assert.Equal(AccessToken, viewModel.AccessToken);
            Assert.Equal(custId, viewModel.CustId);
            Assert.Equal(IsEnableSingleWallet, controller.ViewBag.IsEnableSingleWallet);
        }

        [Fact]
        public async Task Summary_ValidModel_QueryDataFromService()
        {
            //Arrange
            var metalModel = new SummaryStatementMetaModel
            {
                CustId = 100,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                PageSize = 50,
                PageIndex = 100,
                Lang = "en-US",
                BaseCurrency = "USD"
            };
            var fakeData = new SummaryStatementViewModel();
            var fakeService = new Mock<ISummaryStatementService>();
            fakeService.Setup(x => x.QueryAsync(It.IsAny<SummaryStatementMetaModel>()))
            .Returns(Task.FromResult(fakeData));

            //Action
            using (var controller = new StatementController(Mock.Of<ICommonService>()))
            {
                controller.ControllerContext = _controllerContext.Object;
                await controller.Summary(fakeService.Object, metalModel).ConfigureAwait(false);
            }

            //Assert
            fakeService.Verify(x => x.QueryAsync(metalModel), Times.Once());
        }

        [Theory]
        [InlineData(TransDescValue.ModeViewBetStatement, TransDescValue.ModeViewBetStatement, "Betting Statement")]
        [InlineData(TransDescValue.ModeViewCancell, TransDescValue.ModeViewCancell, "Void/Cancelled Bet")]
        [InlineData(TransDescValue.ModeViewOutstanding, TransDescValue.ModeViewOutstanding, "Outstanding Bet")]
        [InlineData(TransDescValue.ModeViewPromotion, TransDescValue.ModeViewPromotion, "Promotion")]
        [InlineData(0, 0, "Statement")]
        public async Task ByGroup_WhenExecute_AlwayAssignCommonViewModelValue(int modeView, int expectedModelView, string expectedTransDesc)
        {
            //Arrange
            const int Custid = 100;
            var metalModel = new StatementByGroupMetaModel
            {
                CustId = Custid,
                Lang = "en-US",
                BaseCurrency = "USD",
                ModeView = modeView
            };
            var fakeData = new StatementByGroupViewModel();
            IActionResult expectedResult = null;
            var fakeService = new Mock<IStatementByGroupService>();
            fakeService.Setup(x => x.QueryAsync(It.IsAny<StatementByGroupMetaModel>()))
            .Returns(Task.FromResult(fakeData));
            //Action
            var controller = new StatementController(_commonService.Object);
            controller.ControllerContext = _controllerContext.Object;
            expectedResult = await controller.ByGroup(fakeService.Object, metalModel).ConfigureAwait(false);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(expectedResult);
            var viewModel = Assert.IsType<StatementByGroupViewModel>(viewResult.Model);
            Assert.Equal(metalModel.BaseCurrency, viewModel.BaseCurrency);
            Assert.Equal(metalModel.Lang, viewModel.Lang);
            Assert.Equal(AccessToken, viewModel.AccessToken);
            Assert.Equal(Custid, viewModel.CustId);
            Assert.Equal(expectedModelView, viewModel.ModeView);
            Assert.Equal(expectedTransDesc, viewModel.TransDesc);
            Assert.Equal(IsEnableSingleWallet, controller.ViewBag.IsEnableSingleWallet);
        }

        [Fact]
        public async Task ByGroup_ValidModel_QueryDataFromService()
        {
            //Arrange
            var metalModel = new StatementByGroupMetaModel
            {
                CustId = 100,
                WinlostDate = DateTime.Now,
                ModeView = 1,
                Lang = "en-US",
                BaseCurrency = "USD"
            };
            var fakeData = FakeHelper.Create<StatementByGroupViewModel>();
            var fakeService = new Mock<IStatementByGroupService>();
            fakeService.Setup(x => x.QueryAsync(It.IsAny<StatementByGroupMetaModel>()))
            .Returns(Task.FromResult(fakeData));

            //Action
            using (var controller = new StatementController(Mock.Of<ICommonService>()))
            {
                controller.ControllerContext = _controllerContext.Object;
                await controller.ByGroup(fakeService.Object, metalModel).ConfigureAwait(false);
            }

            //Assert
            fakeService.Verify(x => x.QueryAsync(metalModel), Times.Once());
        }
    }
}