namespace Fanex.Athena.ExtService.Core.UnitTest.Services
{
    using System.Threading.Tasks;
    using Common.Services;
    using Core.Services;
    using Microsoft.Extensions.Options;
    using Models;
    using Moq;
    using Xunit;

    public class TokenAuthenticationServiceTest
    {
        private readonly IAuthenticateService _authenticateService;
        private readonly Mock<ICommonService> _mockCommonService = new();
        private readonly Mock<IOAuthService> _mockOAuthService = new();

        private readonly Mock<IUserManagementService> _mockUserManagement = new();

        private readonly TokenManagement _tokenManagement = new()
        {
            AccessExpiration = 30,
            Audience = "test",
            Issuer = "test",
            Secret = "04242b2d1c59118f33c9e3d01a776a7e50b9495b7dffbff6525c6f6cd7755d16",
            RefreshExpiration = 30
        };

        public TokenAuthenticationServiceTest()
        {
            var mockTokenManagement = new Mock<IOptions<TokenManagement>>();
            mockTokenManagement.Setup(x => x.Value).Returns(_tokenManagement);
            _mockCommonService.Setup(_ => _.GetSiteDBServer(It.IsAny<string>())).Returns(Task.FromResult(string.Empty));

            _authenticateService = new TokenAuthenticationService(
                _mockUserManagement.Object,
                _mockOAuthService.Object,
                mockTokenManagement.Object);
        }

        [Theory]
        [InlineData("asx", "1234aa")]
        [InlineData("12bet@asx", "1234aa")]
        public async Task IsAuthenticated_InValidUser_ReturnFalse(string userName, string pass)
        {
            //Arrange
            var token = new TokenRequest
            {
                Username = userName,
                Password = pass
            };
            _mockUserManagement.Setup(x => x.IsValidUser(token.Username, token.Password)).Returns(false);
            //Action
            var result = await _authenticateService.IsAuthenticated(token);
            //Assert

            Assert.False(result.Item1);
        }

        [Theory]
        [InlineData("asx", "1234aa")]
        [InlineData("12bet@asx", "1234aa")]
        public async Task IsAuthenticated_ValidUser_ReturnTrueAndNotNullToken(string userName, string pass)
        {
            //Arrange
            var token = new TokenRequest
            {
                Username = userName,
                Password = pass
            };
            _mockUserManagement.Setup(x => x.IsValidUser(It.IsAny<string>(), token.Password)).Returns(true);
            _mockOAuthService.Setup(_ => _.RequestTokenAsync(token)).Returns(Task.FromResult("token"));

            //Action
            var result = await _authenticateService.IsAuthenticated(token);
            //Assert

            Assert.True(result.Item1);
            Assert.NotNull(result.Item2);
            Assert.True(result.Item2.ExpiresIn == 0);
            Assert.True(string.IsNullOrEmpty(result.Item2.TokenType));
        }
    }
}