using System.Threading.Tasks;

using Fanex.Athena.ExtService.BetList.Controllers;
using Fanex.Athena.ExtService.BetList.Models.MetaModel;
using Fanex.Athena.ExtService.BetList.Models.ViewModel;

using Microsoft.AspNetCore.Mvc;

using Moq;

using Xunit;

namespace Fanex.Athena.ExtService.BetList.UnitTests.Controllers
{
    public class BetListDetailByProductControllerTest : BaseControllerTest
    {
        [Fact]
        public async Task Detail_WhenCall_RetrunCorrectViewModelAsync()
        {
            //Arrange
            const string Title = "Bet List - Product";
            var metalModel = new BetListDetailByProductMetaModel
            {
                BaseCurrency = "ANY",
                Lang = "en-US"
            };
            var fakeResult = new BetListResultViewModel();
            IActionResult result = null;
            _betListApiClient.Setup(x => x.PostAsync<BetListResultViewModel>(
                "/BetList/GetBetList", It.IsAny<BetListDetailByProductMetaModel>(), It.IsAny<string>(), 0))
                .Returns(Task.FromResult(fakeResult));

            var licenseeInfo = await _commonService.Object.GetLicenseeInfoAsync(SiteName, 0);

            //Action
            using (var controller = new BetListDetailByProductController(_commonService.Object
                , _betListApiClient.Object))
            {
                controller.ControllerContext = _controllerContext.Object;
                result = await controller.Detail(metalModel).ConfigureAwait(false);
            }

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<BetListResultViewModel>(viewResult.Model);
            var title = Assert.IsAssignableFrom<string>(viewResult.ViewData["Title"]);
            Assert.NotNull(viewModel);

            Assert.Equal(viewModel.Info, licenseeInfo);
            Assert.Equal(viewModel.BaseCurrency, metalModel.BaseCurrency);
            Assert.Equal(viewModel.Lang, metalModel.Lang);
            Assert.Equal(viewModel.AccessToken, AccessToken);
            Assert.Equal(title, Title);
        }

    }
}
