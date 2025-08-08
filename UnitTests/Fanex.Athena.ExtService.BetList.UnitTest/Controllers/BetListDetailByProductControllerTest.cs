namespace Fanex.Athena.ExtService.BetList.UnitTests.Controllers
{
    using System.Threading.Tasks;
    using BetList.Controllers;
    using BetList.Models.DTO;
    using BetList.Models.MetaModel;
    using BetList.Models.ViewModel;
    using ExtService.UnitTest.Common.Helpers;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Xunit;

    public class BetListDetailByProductControllerTest : BaseControllerTest
    {
        [Fact]
        public async Task Detail_WhenCall_ReturnCorrectViewModelAsync()
        {
            //Arrange
            const string Title = "Bet List - Product";
            var metalModel = FakeHelper.Create<BetListDetailByProductMetaModel>();
            metalModel.BaseCurrency = "AMY";
            metalModel.Lang = "en-US";

            var fakeResult = FakeHelper.Create<BetListResultViewModel>(result => result.Total,
                                                                       FakeHelper.Create<BetListTotalData>());

            IActionResult result = null;
            _betListApiClient.Setup(x => x.PostAsync<BetListResultViewModel>(
                "/BetList/GetBetList", It.IsAny<BetListDetailByProductMetaModel>(),
                It.IsAny<string>()))
                .Returns(Task.FromResult(fakeResult));

            var licenseeInfo = await _commonService.Object.GetLicenseeInfoAsync(SiteName);

            //Action
            using (var controller = new BetListDetailByProductController
                (_commonService.Object, _betListApiClient.Object, _browserService.Object, _contextService.Object))
            {
                controller.ControllerContext = _controllerContext.Object;
                result = await controller.Detail(metalModel).ConfigureAwait(false);
            }

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<BetListResultViewModel>(viewResult.Model);
            var title = Assert.IsAssignableFrom<string>(viewResult.ViewData["Title"]);
            Assert.NotNull(viewModel);
            Assert.Equal(fakeResult.Main, viewModel.Main);
            Assert.Equal(fakeResult.Total, viewModel.Total);
            Assert.Equal(fakeResult.LeagueName, viewModel.LeagueName);
            Assert.Equal(fakeResult.HasLicWl, viewModel.HasLicWl);

            Assert.Equal(licenseeInfo, viewModel.Info);
            Assert.Equal(metalModel.BaseCurrency, viewModel.BaseCurrency);
            Assert.Equal(metalModel.Lang, viewModel.Lang);
            Assert.Equal(AccessToken, viewModel.AccessToken);
            Assert.Equal(Title, title);
        }
    }
}