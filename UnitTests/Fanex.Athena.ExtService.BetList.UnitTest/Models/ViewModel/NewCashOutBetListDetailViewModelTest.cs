namespace Fanex.Athena.ExtService.BetList.UnitTests.Models.ViewModel
{
    using BetList.Models.ViewModel;
    using ExtService.UnitTest.Common.Helpers;
    using Xunit;

    public class NewCashOutBetListDetailViewModelTest
    {
        [Fact]
        public void DetailHistory_EmptyCashOutExtraInfo_ReturnDefaultList()
        {
            //Arrange
            var viewModel = FakeHelper.Create<NewCashOutBetListDetailViewModel>();
            viewModel.CashoutExtraInfo = string.Empty;

            //Action
            var result = viewModel.DetailHistory;

            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public void DetailHistory_ValidCashOutExtraInfo_ReturnCashOutHistoryDetailList()
        {
            //Arrange
            var viewModel = FakeHelper.Create<NewCashOutBetListDetailViewModel>();
            viewModel.CashoutExtraInfo = @"<Root>
										  <Any ct=""2000-12-16"" w=""1"" s=""1""/>
									 </Root>";

            //Action
            var result = viewModel.DetailHistory;

            //Assert
            Assert.NotEmpty(result);
        }
    }
}