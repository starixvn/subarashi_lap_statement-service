namespace Fanex.Athena.ExtService.UnitTests.Controllers
{
    using Common.Models.ViewModels;
    using ExtService.Controllers;
    using Microsoft.AspNetCore.Mvc;
    using Xunit;

    public class ErrorControllerTest
    {
        [Theory]
        [InlineData(100)]
        [InlineData(200)]
        public void Index_WhenCall_ReturnCorrectViewModelAndCode(int code)
        {
            // Arrange
            var viewModel = new BaseViewModel
            {
                AccessToken = "Not token",
                BaseCurrency = string.Empty,
                Lang = string.Empty
            };

            var controller = new ErrorController();

            // Action
            var result = controller.Index(code) as ViewResult;
            var resultModel = result.Model as BaseViewModel;

            // Assert
            Assert.Equal(code, (int)result.ViewData["Code"]);
            Assert.Equal(viewModel.AccessToken, resultModel.AccessToken);
            Assert.Equal(viewModel.BaseCurrency, resultModel.BaseCurrency);
            Assert.Equal(viewModel.Lang, resultModel.Lang);
        }
    }
}