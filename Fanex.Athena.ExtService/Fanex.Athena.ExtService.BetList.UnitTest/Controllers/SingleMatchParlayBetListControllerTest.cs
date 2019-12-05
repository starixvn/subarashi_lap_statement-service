namespace Fanex.Athena.ExtService.BetList.UnitTests.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.ExtService.BetList.Controllers;
    using Fanex.Athena.ExtService.BetList.Models.MetaModel;
    using Microsoft.AspNetCore.Mvc;
    using Xunit;

    public class SingleMatchParlayBetListControllerTest : BaseControllerTest
    {
        [Fact]
        public async Task Detail_WhenCall_RetrunCorrectResultAsync()
        {
            // Arrange
            var metaModel = new SingleMatchParlayDetailMetaModel
            {
                Lang = "en-US"
            };

            var expected = new List<string> { "Text1", "Text2" };

            _betListApiClient.Setup(x => x.PostAsync<IList<string>>("/SingleMatchParlayBetList/Detail", metaModel, metaModel.Lang, 0))
                .Returns(Task.FromResult<IList<string>>(expected));

            IActionResult result = null;
            
            // Action
            using (var controller = new SingleMatchParlayBetListController(_betListApiClient.Object))
            {
                result = await controller.Detail(metaModel).ConfigureAwait(false);               
            }

            //Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsAssignableFrom<IList<string>>(jsonResult.Value);

            Assert.Equal(actualResult.Count, expected.Count);
            Assert.Equal(string.Join(",", actualResult), string.Join(",", expected));
        }
    }
}
