namespace Fanex.Athena.ExtService.Core.UnitTests.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using Common.Services;
    using Core.Attributes;
    using ExtService.UnitTest.Common.Helpers;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Options;
    using Models.DTO;
    using Moq;
    using Xunit;

    public class IPFilterAttributeTest
    {
        private readonly Mock<IOptions<ApplicationOptions>> _options;
        private readonly Mock<ICommonService> _commonService;
        private readonly ActionExecutingContext _actionExecutingContext;
        private readonly ActionExecutionDelegate _actionExecutingDelegate;
        private readonly Mock<HttpContext> _mockHttpContext;

        public IPFilterAttributeTest()
        {
            var modelState = new ModelStateDictionary();
            _mockHttpContext = new Mock<HttpContext>();

            var actionContext = new ActionContext(
                  _mockHttpContext.Object,
                  Mock.Of<RouteData>(),
                  Mock.Of<ActionDescriptor>(),
                  modelState
              );
            _actionExecutingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                Mock.Of<Controller>());

            _actionExecutingDelegate = () => Task.FromResult(new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), Mock.Of<Controller>()));
            _options = new Mock<IOptions<ApplicationOptions>>();
            _commonService = new Mock<ICommonService>();
            _commonService.Setup(x => x.GetWhitelistIPFromDB(It.IsAny<string>())).Returns(Task.FromResult(new List<string>()));
            var httpRequest = new Mock<HttpRequest>();
            httpRequest.Setup(_ => _.Host).Returns(HostString.FromUriComponent("asdsadsadsads.com")); //Make sure not from internal
            httpRequest.Setup(_ => _.HttpContext.User.Identity.IsAuthenticated).Returns(false); // Make sure not login
            _mockHttpContext.Setup(_ => _.Request).Returns(httpRequest.Object);
            _mockHttpContext.Setup(_ => _.User).Returns(httpRequest.Object.HttpContext.User);
        }

        [Fact]
        public async Task OnActionExecuting_WhenEmptyWhiteListIP_NoChangeResultAsync()
        {
            //Arrange
            var fakeContent = "NOCHANGE";
            var fakeResult = new ContentResult { Content = fakeContent };
            var applicationOption = FakeHelper.Create<ApplicationOptions>(options => options.WhiteList, new List<string>());

            _options.Setup(x => x.Value).Returns(applicationOption);
            _actionExecutingContext.Result = fakeResult;
            var connectionInfo = new Mock<ConnectionInfo>();
            connectionInfo.Setup(x => x.RemoteIpAddress).Returns(new IPAddress(0x2414188f));
            _mockHttpContext.Setup(x => x.Connection).Returns(connectionInfo.Object);
            //Action
            var filter = new IPFilterAttribute(_options.Object, _commonService.Object);

            await filter.OnActionExecutionAsync(_actionExecutingContext, _actionExecutingDelegate);

            //Assert
            var actualResult = Assert.IsAssignableFrom<ContentResult>(_actionExecutingContext.Result);

            Assert.Equal(fakeContent, actualResult.Content);
        }

        [Fact]
        public async Task OnActionExecuting_WhenAllowIP_WhiteListSingle_NoChangeResultAsync()
        {
            //Arrange
            const int requestIp = 0x2414188f;
            const string whitelistIp = "143.24.20.36";

            const string fakeContent = "NOCHANGE";
            var fakeResult = new ContentResult { Content = fakeContent };
            var applicationOption = FakeHelper.Create<ApplicationOptions>(options => options.WhiteList, new List<string> { whitelistIp });

            _options.Setup(x => x.Value).Returns(applicationOption);
            _actionExecutingContext.Result = fakeResult;
            var connectionInfo = new Mock<ConnectionInfo>();
            connectionInfo.Setup(x => x.RemoteIpAddress).Returns(new IPAddress(requestIp));
            _mockHttpContext.Setup(x => x.Connection).Returns(connectionInfo.Object);
            //Action
            var filter = new IPFilterAttribute(_options.Object, _commonService.Object);

            await filter.OnActionExecutionAsync(_actionExecutingContext, _actionExecutingDelegate);

            //Assert
            var actualResult = Assert.IsAssignableFrom<ContentResult>(_actionExecutingContext.Result);

            Assert.Equal(fakeContent, actualResult.Content);
        }

        [Fact]
        public async Task OnActionExecuting_WhenNotAllowIP_WhiteListSingle_ReturnResultWithDeniedMessageAsync()
        {
            //Arrange
            const long requestIpNumber = 169003217;
            const string requestIp = "209.200.18.10";
            const string whitelistIp = "143.24.20.36";

            var expectedResult = $"The IP Address: {requestIp} was blocked";
            var applicationOption = FakeHelper.Create<ApplicationOptions>(options => options.WhiteList, new List<string> { whitelistIp });
            _options.Setup(x => x.Value).Returns(applicationOption);

            var connectionInfo = new Mock<ConnectionInfo>();
            connectionInfo.Setup(x => x.RemoteIpAddress).Returns(new IPAddress(requestIpNumber));

            _mockHttpContext.Setup(x => x.Connection).Returns(connectionInfo.Object);

            //Action
            var filter = new IPFilterAttribute(_options.Object, _commonService.Object);

            await filter.OnActionExecutionAsync(_actionExecutingContext, _actionExecutingDelegate);

            //Assert
            var actualResult = Assert.IsAssignableFrom<ContentResult>(_actionExecutingContext.Result);

            Assert.Equal(expectedResult, actualResult.Content);
        }

        [Theory]
        [InlineData("119.81.247.201")]
        [InlineData("119.81.247.202")]
        [InlineData("119.81.247.203")]
        [InlineData("119.81.247.204")]
        [InlineData("119.81.247.205")]
        [InlineData("119.81.247.206")]
        [InlineData("119.81.247.207")]
        public async Task OnActionExecuting_WhenAllowIP_WhiteListRange_NoChangeResultAsync(string requestIp)
        {
            //Arrange
            const string whitelistIp = "119.81.247.200/29";

            const string fakeContent = "NOCHANGE";
            var fakeResult = new ContentResult { Content = fakeContent };

            var applicationOption = FakeHelper.Create<ApplicationOptions>(options => options.WhiteList, new List<string> { whitelistIp });
            _options.Setup(x => x.Value).Returns(applicationOption);
            _actionExecutingContext.Result = fakeResult;
            var connectionInfo = new Mock<ConnectionInfo>();
            connectionInfo.Setup(x => x.RemoteIpAddress).Returns(IPAddress.Parse(requestIp));
            _mockHttpContext.Setup(x => x.Connection).Returns(connectionInfo.Object);

            //Action
            var filter = new IPFilterAttribute(_options.Object, _commonService.Object);

            await filter.OnActionExecutionAsync(_actionExecutingContext, _actionExecutingDelegate);

            //Assert
            var actualResult = Assert.IsAssignableFrom<ContentResult>(_actionExecutingContext.Result);

            Assert.Equal(fakeContent, actualResult.Content);
        }

        [Theory]
        [InlineData("119.81.247.199")]
        [InlineData("119.81.247.208")]
        public async Task OnActionExecuting_WhenNotAllowIP_WhiteListRange_ReturnResultWithDeniedMessageAsync(string requestIp)
        {
            //Arrange
            const string whitelistIp = "119.81.247.200/29";

            var expectedResult = $"The IP Address: {requestIp} was blocked";
            var applicationOption = FakeHelper.Create<ApplicationOptions>(options => options.WhiteList, new List<string> { whitelistIp });
            _options.Setup(x => x.Value).Returns(applicationOption);

            var connectionInfo = new Mock<ConnectionInfo>();
            connectionInfo.Setup(x => x.RemoteIpAddress).Returns(IPAddress.Parse(requestIp));

            _mockHttpContext.Setup(x => x.Connection).Returns(connectionInfo.Object);

            //Action
            var filter = new IPFilterAttribute(_options.Object, _commonService.Object);

            await filter.OnActionExecutionAsync(_actionExecutingContext, _actionExecutingDelegate);

            //Assert
            var actualResult = Assert.IsAssignableFrom<ContentResult>(_actionExecutingContext.Result);

            Assert.Equal(expectedResult, actualResult.Content);
        }

        [Theory]
        [InlineData("119.81.247.256")]
        [InlineData("119.81.247.200/100")]
        public async Task OnActionExecuting_WhiteListIPInvalid_ThrowExceptionWithMessage(string whiteListIp)
        {
            //Arrange
            const string requestIP = "119.81.247.200";

            var expectedResult = $"The WhiteList IP: {whiteListIp} was invalid - client IP: ${requestIP}";
            var applicationOption = FakeHelper.Create<ApplicationOptions>(options => options.WhiteList, new List<string> { whiteListIp });
            _options.Setup(x => x.Value).Returns(applicationOption);

            var connectionInfo = new Mock<ConnectionInfo>();
            connectionInfo.Setup(x => x.RemoteIpAddress).Returns(IPAddress.Parse(requestIP));

            _mockHttpContext.Setup(x => x.Connection).Returns(connectionInfo.Object);

            //Action
            var filter = new IPFilterAttribute(_options.Object, _commonService.Object);

            var ex = await Assert.ThrowsAsync<Exception>(() => filter.OnActionExecutionAsync(_actionExecutingContext, _actionExecutingDelegate));

            //Assert
            Assert.Equal(expectedResult, ex.Message);
        }
    }
}