namespace Fanex.Athena.ExtService.Core.UnitTests.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Security.Claims;
    using Common.Constant;
    using Core.Services;
    using Microsoft.AspNetCore.Http;
    using Moq;
    using Newtonsoft.Json;
    using Xunit;

    public class ContextServiceTest
    {
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly Mock<HttpContext> _mockHttpContext;
        private readonly Mock<ClaimsPrincipal> _mockUser;
        private readonly Mock<ConnectionInfo> _mockConnectionInfo;
        private readonly ContextService _contextService;
        private readonly List<Claim> _claims;

        public ContextServiceTest()
        {
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockHttpContext = new Mock<HttpContext>();
            _mockUser = new Mock<ClaimsPrincipal>();
            _mockConnectionInfo = new Mock<ConnectionInfo>();
            _claims = new List<Claim>();

            _mockHttpContext.Setup(x => x.User).Returns(_mockUser.Object);
            _mockHttpContext.Setup(x => x.Connection).Returns(_mockConnectionInfo.Object);
            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(_mockHttpContext.Object);

            _contextService = new ContextService(_mockHttpContextAccessor.Object);
        }

        [Fact]
        public void IsAdministrator_WhenClaimExists_ReturnsTrueValue()
        {
            // Arrange
            SetupClaim("Administrator", "true");

            // Act
            var result = _contextService.IsAdministrator;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsAdministrator_WhenClaimExistsWithFalse_ReturnsFalseValue()
        {
            // Arrange
            SetupClaim("Administrator", "false");

            // Act
            var result = _contextService.IsAdministrator;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsAdministrator_WhenClaimDoesNotExist_ReturnsDefault()
        {
            // Arrange
            SetupNoClaims();

            // Act
            var result = _contextService.IsAdministrator;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ClientIP_ReturnsRemoteIPAddress()
        {
            // Arrange
            var expectedIP = "192.168.1.1";
            _mockConnectionInfo.Setup(x => x.RemoteIpAddress)
                .Returns(IPAddress.Parse(expectedIP));

            // Act
            var result = _contextService.ClientIP;

            // Assert
            Assert.Equal(expectedIP, result);
        }

        [Fact]
        public void OAuthAccessToken_ReturnsTokenFromClaim()
        {
            // Arrange
            var expectedToken = "test-token-123";
            SetupClaim(CustomClaimType.IdentityAccessToken, expectedToken);

            // Act
            var result = _contextService.OAuthAccessToken;

            // Assert
            Assert.Equal(expectedToken, result);
        }

        [Fact]
        public void SiteName_ReturnsSiteNameFromClaim()
        {
            // Arrange
            var expectedSiteName = "TestSite";
            SetupClaim("SiteName", expectedSiteName);

            // Act
            var result = _contextService.SiteName;

            // Assert
            Assert.Equal(expectedSiteName, result);
        }

        [Fact]
        public void UserName_ReturnsUserIDFromClaim()
        {
            // Arrange
            var expectedUserName = "testuser";
            SetupClaim("UserID", expectedUserName);

            // Act
            var result = _contextService.UserName;

            // Assert
            Assert.Equal(expectedUserName, result);
        }

        [Theory]
        [InlineData("true", true)]
        [InlineData("false", false)]
        [InlineData("True", true)]
        [InlineData("False", false)]
        public void IsAllowedCustomerComm_ReturnsCorrectBooleanValue(string claimValue, bool expected)
        {
            // Arrange
            SetupClaim("IsAllowedCustomerComm", claimValue);

            // Act
            var result = _contextService.IsAllowedCustomerComm;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("1", 1)]
        [InlineData("42", 42)]
        [InlineData("0", 0)]
        public void SpecialPlatform_ReturnsCorrectIntegerValue(string claimValue, int expected)
        {
            // Arrange
            SetupClaim("SpecialPlatform", claimValue);

            // Act
            var result = _contextService.SpecialPlatform;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void OfferedSubProductList_WhenClaimExists_ReturnsCommaSeparatedValues()
        {
            // Arrange
            var offeredProducts = "Product1,Product2,Product3";
            SetupClaim("OfferedSubProducts", offeredProducts);

            // Act
            var result = _contextService.OfferedSubProductList;

            // Assert
            Assert.Equal(3, result.Count());
            Assert.Contains("Product1", result);
            Assert.Contains("Product2", result);
            Assert.Contains("Product3", result);
        }

        [Fact]
        public void OfferedSubProductList_WhenClaimIsEmpty_ReturnsEmptyEnumerable()
        {
            // Arrange
            SetupClaim("OfferedSubProducts", "");

            // Act
            var result = _contextService.OfferedSubProductList;

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void OfferedSubProductList_WhenClaimDoesNotExist_ReturnsEmptyEnumerable()
        {
            // Arrange
            SetupNoClaims();

            // Act
            var result = _contextService.OfferedSubProductList;

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void IsInRoles_WhenUserIsInRole_ReturnsTrue()
        {
            // Arrange
            var roles = new[] { "Admin", "User" };
            _mockUser.Setup(x => x.IsInRole("Admin")).Returns(true);

            // Act
            var result = _contextService.IsInRoles(roles);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsInRoles_WhenUserIsNotInAnyRole_ReturnsFalse()
        {
            // Arrange
            var roles = new[] { "Admin", "User" };
            _mockUser.Setup(x => x.IsInRole(It.IsAny<string>())).Returns(false);

            // Act
            var result = _contextService.IsInRoles(roles);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AgentList_WithCurrencyAgentClaims_ReturnsParsedDictionary()
        {
            // Arrange
            var currencyAgentInfo1 = new { CurrencyId = 1, AgentId = new[] { 100, 101 }, Currency = "USD" };
            var currencyAgentInfo2 = new { CurrencyId = 2, AgentId = new[] { 200, 201 }, Currency = "EUR" };
            
            var claims = new List<Claim>
            {
                new Claim("CurrencyAgent", JsonConvert.SerializeObject(currencyAgentInfo1)),
                new Claim("CurrencyAgent", JsonConvert.SerializeObject(currencyAgentInfo2))
            };

            SetupClaimsEnumerable(claims);

            // Act
            var result = _contextService.AgentList;

            // Assert
            Assert.Equal(2, result.Count);
            Assert.True(result.ContainsKey(1));
            Assert.True(result.ContainsKey(2));
            Assert.Equal(new[] { 100, 101 }, result[1]);
            Assert.Equal(new[] { 200, 201 }, result[2]);
        }

        [Fact]
        public void CurrencyList_WithCurrencyAgentClaims_ReturnsParsedDictionary()
        {
            // Arrange
            var currencyAgentInfo1 = new { CurrencyId = 1, AgentId = new[] { 100, 101 }, Currency = "USD" };
            var currencyAgentInfo2 = new { CurrencyId = 2, AgentId = new[] { 200, 201 }, Currency = "EUR" };
            
            var claims = new List<Claim>
            {
                new Claim("CurrencyAgent", JsonConvert.SerializeObject(currencyAgentInfo1)),
                new Claim("CurrencyAgent", JsonConvert.SerializeObject(currencyAgentInfo2))
            };

            SetupClaimsEnumerable(claims);

            // Act
            var result = _contextService.CurrencyList;

            // Assert
            Assert.Equal(2, result.Count);
            Assert.True(result.ContainsKey(1));
            Assert.True(result.ContainsKey(2));
            Assert.Equal("USD", result[1]);
            Assert.Equal("EUR", result[2]);
        }

        [Fact]
        public void AgentList_AccessedMultipleTimes_ReturnsSameInstance()
        {
            // Arrange
            var currencyAgentInfo = new { CurrencyId = 1, AgentId = new[] { 100 }, Currency = "USD" };
            var claims = new List<Claim>
            {
                new Claim("CurrencyAgent", JsonConvert.SerializeObject(currencyAgentInfo))
            };
            SetupClaimsEnumerable(claims);

            // Act
            var result1 = _contextService.AgentList;
            var result2 = _contextService.AgentList;

            // Assert
            Assert.Same(result1, result2);
        }

        [Fact]
        public void CurrencyList_AccessedMultipleTimes_ReturnsSameInstance()
        {
            // Arrange
            var currencyAgentInfo = new { CurrencyId = 1, AgentId = new[] { 100 }, Currency = "USD" };
            var claims = new List<Claim>
            {
                new Claim("CurrencyAgent", JsonConvert.SerializeObject(currencyAgentInfo))
            };
            SetupClaimsEnumerable(claims);

            // Act
            var result1 = _contextService.CurrencyList;
            var result2 = _contextService.CurrencyList;

            // Assert
            Assert.Same(result1, result2);
        }

        [Theory]
        [InlineData("IsShowIP", true)]
        [InlineData("IsShowIP", false)]
        public void IsShowIP_ReturnsCorrectValue(string claimType, bool expectedValue)
        {
            // Arrange
            SetupClaim(claimType, expectedValue.ToString().ToLower());

            // Act
            var result = _contextService.IsShowIP;

            // Assert
            Assert.Equal(expectedValue, result);
        }

        [Theory]
        [InlineData("IShowIPVN", true)]
        [InlineData("IShowIPVN", false)]
        public void IsShowIpvn_ReturnsCorrectValue(string claimType, bool expectedValue)
        {
            // Arrange
            SetupClaim(claimType, expectedValue.ToString().ToLower());

            // Act
            var result = _contextService.IsShowIpvn;

            // Assert
            Assert.Equal(expectedValue, result);
        }

        [Fact]
        public void SiteDBServer_ReturnsValueFromClaim()
        {
            // Arrange
            var expectedServer = "TestDBServer";
            SetupClaim("SiteDBServer", expectedServer);

            // Act
            var result = _contextService.SiteDBServer;

            // Assert
            Assert.Equal(expectedServer, result);
        }

        [Fact]
        public void IsFakeLicensee_ReturnsCorrectBooleanValue()
        {
            // Arrange
            SetupClaim("IsFakeLicensee", "true");

            // Act
            var result = _contextService.IsFakeLicensee;

            // Assert
            Assert.True(result);
        }

        private void SetupClaim(string claimType, string value)
        {
            _claims.Clear();
            _claims.Add(new Claim(claimType, value));
            _mockUser.Setup(x => x.FindFirst(claimType))
                .Returns(new Claim(claimType, value));
            _mockUser.Setup(x => x.Claims).Returns(_claims);
        }

        private void SetupNoClaims()
        {
            _mockUser.Setup(x => x.FindFirst(It.IsAny<string>()))
                .Returns((Claim)null);
            _mockUser.Setup(x => x.Claims).Returns(Enumerable.Empty<Claim>());
        }

        private void SetupClaimsEnumerable(IEnumerable<Claim> claims)
        {
            _mockUser.Setup(x => x.Claims).Returns(claims);
        }
    }
}