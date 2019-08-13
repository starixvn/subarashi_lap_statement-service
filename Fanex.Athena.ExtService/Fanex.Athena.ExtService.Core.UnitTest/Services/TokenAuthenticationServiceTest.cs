using Fanex.Athena.ExtService.Core.Models;
using Fanex.Athena.ExtService.Core.Services;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Fanex.Athena.ExtService.Core.UnitTest.Services
{
    public class TokenAuthenticationServiceTest
    {
        private readonly IAuthenticateService _authenticateService;
        private TokenManagement _tokenManagement = new TokenManagement
        {
            AccessExpiration = 30,
            Audience = "test",
            Issuer = "test",
            Secret = "04242b2d1c59118f33c9e3d01a776a7e50b9495b7dffbff6525c6f6cd7755d16",
            RefreshExpiration = 30
        };
        private Mock<IUserManagementService> _mockUserManagement = new Mock<IUserManagementService>();
        public TokenAuthenticationServiceTest()
        {
            var mockTokenManagement = new Mock<IOptions<TokenManagement>>();
            mockTokenManagement.Setup(x => x.Value).Returns(_tokenManagement);
            _authenticateService = new TokenAuthenticationService(_mockUserManagement.Object, mockTokenManagement.Object);
        }

        [Fact]
        public void IsAuthenticated_InValidUser_ReturnFalse()
        {
            //Arrange
            var token = new TokenRequest
            {
                Username = "asx",
                Password = "13424"
            };
            _mockUserManagement.Setup(x => x.IsValidUser(token.Username, token.Password)).Returns(false);
            //Action
            var result = _authenticateService.IsAuthenticated(token, out Token tokenResult);
            //Assert

            Assert.False(result);
        }

        [Fact]
        public void IsAuthenticated_ValidUser_ReturnTrueAndNotNullToken()
        {
            //Arrange
            var token = new TokenRequest
            {
                Username = "asx",
                Password = "13424"
            };
            _mockUserManagement.Setup(x => x.IsValidUser(token.Username, token.Password)).Returns(true);
            //Action
            var result = _authenticateService.IsAuthenticated(token, out Token tokenResult);
            //Assert

            Assert.True(result);
            Assert.NotNull(tokenResult.AccessToken);
        }
    }
}
