using System.Threading.Tasks;

using Fanex.Athena.ExtService.BetList.Controllers;
using Fanex.Athena.ExtService.BetList.Models.MetaModel;
using Fanex.Athena.ExtService.BetList.Models.ViewModel;

using Microsoft.AspNetCore.Mvc;

using Moq;

using Xunit;

namespace Fanex.Athena.ExtService.BetList.UnitTests.Controllers
{
    public class ColossusBetBetListControllerTest : BaseControllerTest
    {
        [Fact]
        public async Task Detail_WhenCall_RetrunCorrectViewModelAsync()
        {
            //Arrange
            const string Title = "Colossus Bet";
            var metalModel = new ColossusBetDetailMetaModel
            {
                BaseCurrency = "ANY",
                Lang = "en-US"
            };
            var fakeResult = new ColossusBetDetailViewModel();
            IActionResult result = null;
            _betListApiClient.Setup(x => x.PostAsync<ColossusBetDetailViewModel>(
                "/ColossusBetBetList/Detail", It.IsAny<ColossusBetDetailMetaModel>(),
                It.IsAny<string>(), 0))
                .Returns(Task.FromResult(fakeResult));
            
            //Action
            using (var controller = new ColossusBetBetListController(_commonService.Object
                , _betListApiClient.Object))
            {
                controller.ControllerContext = _controllerContext.Object;
                result = await controller.Detail(metalModel).ConfigureAwait(false);
            }

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<ColossusBetDetailViewModel>(viewResult.Model);
            var title = Assert.IsAssignableFrom<string>(viewResult.ViewData["Title"]);

            Assert.NotNull(metalModel.AgentListXml);
            Assert.NotNull(viewModel);

            Assert.Equal(viewModel.BaseCurrency, metalModel.BaseCurrency);
            Assert.Equal(viewModel.Lang, metalModel.Lang);
            Assert.Equal(viewModel.AccessToken, AccessToken);
            Assert.Equal(title, Title);
        }

    }
}
