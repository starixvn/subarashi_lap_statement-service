using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fanex.Athena.ExtService.Common.Extensions;
using Fanex.Athena.ExtService.Common.Models.DTO;
using Fanex.Athena.ExtService.Common.Services;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Xunit;

namespace Fanex.Athena.ExtService.Common.UnitTests.Services
{
    public class BetListApiClientTest
    {
        private readonly Mock<IRestClient> _mockClient;
        private readonly Mock<IOptions<AppSettings>> _mockOptions;
        private readonly Mock<IRestResponse> _mockResponse;
        const string BaseUrl = "http://localhost:5000";
        const string TokenValue = "ANYTOKEN";
        public BetListApiClientTest()
        {
            _mockClient = new Mock<IRestClient>();
            _mockOptions = new Mock<IOptions<AppSettings>>();
            _mockOptions.Setup(x => x.Value).Returns(new AppSettings
            {
                BetListApiHostUrl = BaseUrl,
                BetListApiTokenValue = TokenValue,
            });
            _mockResponse = new Mock<IRestResponse>();
        }
        [Fact]
        public async Task HandleAsync_WhenCallSuccess_PassCorrectParamAndReturnNotNullResult()
        {
            //Arrange
            const string language = "en-US";
            const string FakeContent = "ANY";
            const int timeout = 2;
            IRestRequest restRequest = null;
            _mockResponse.Setup(x => x.ResponseStatus).Returns(ResponseStatus.Completed);
            _mockResponse.Setup(x => x.Content).Returns(FakeContent);
            _mockClient.Setup(x => x.ExecuteTaskAsync(It.IsAny<IRestRequest>())).Callback<IRestRequest>(x => { restRequest = x; })
                .Returns(Task.FromResult(_mockResponse.Object));
            var apiClient = new BetListApiClient(_mockOptions.Object, _mockClient.Object);
            //Action
            var result = await apiClient.PostAsyncString("/test", new { data = "test" }, language, timeout).ConfigureAwait(false);

            //Assert
            Assert.NotNull(restRequest);
            Assert.Equal(Method.POST, restRequest.Method);
            Assert.IsType<RestSharpNewtonsoftJsonSerializer>(restRequest.JsonSerializer);
            Assert.NotNull(result);
            Assert.Equal(FakeContent, result);
            _mockClient.VerifySet(x => x.Timeout = 60000 * timeout);

        }

        [Fact]
        public async Task HandleAsync_WhenCallWithException_ThrowInvalidOperationException()
        {
            //Arrange
            const string language = "en-US";
            const int timeout = 2;
            const string fakeErrorMsg = "Any";
            var actualErrorMsg = $"Http Error ={fakeErrorMsg}\n Error occur when query betlist data";
            var actualException = new Exception();
            _mockResponse.Setup(x => x.ErrorMessage).Returns(fakeErrorMsg);
            _mockResponse.Setup(x => x.ErrorException).Returns(actualException);
            IRestRequest restRequest = null;
            _mockClient.Setup(x => x.ExecuteTaskAsync(It.IsAny<IRestRequest>())).Callback<IRestRequest>(x => { restRequest = x; })
                .Returns(Task.FromResult(_mockResponse.Object));
            var apiClient = new BetListApiClient(_mockOptions.Object, _mockClient.Object);
            //Action
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await apiClient.PostAsyncString("/test", new { data = "test" }, language, timeout).ConfigureAwait(false));

            //Assert

            Assert.Equal(ex.Message, actualErrorMsg);
            Assert.Equal(ex.InnerException, actualException);

        }

        [Fact]
        public async Task PostAsync_WhenCall_ReturnCorrectDataTypeAsync()
        {
            //Arrange
            const string language = "en-US";
            var fakeContent = new { ID = 1, Name = "alex" };
            const int timeout = 2;
            IRestRequest restRequest = null;
            _mockResponse.Setup(x => x.ResponseStatus).Returns(ResponseStatus.Completed);
            _mockResponse.Setup(x => x.Content).Returns(JsonConvert.SerializeObject(fakeContent));
            _mockClient.Setup(x => x.ExecuteTaskAsync(It.IsAny<IRestRequest>())).Callback<IRestRequest>(x => { restRequest = x; })
                .Returns(Task.FromResult(_mockResponse.Object));
            var apiClient = new BetListApiClient(_mockOptions.Object, _mockClient.Object);
            //Action
            var result = await apiClient.PostAsync<dynamic>("/test", new { data = "test" }, language, timeout).ConfigureAwait(false);

            //Assert
            var actualResult = Assert.IsAssignableFrom<JObject>(result);
            Assert.Equal(JsonConvert.SerializeObject(fakeContent), JsonConvert.SerializeObject(actualResult));
        }
    }
}
