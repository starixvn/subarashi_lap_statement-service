namespace Fanex.Athena.ExtService.UnitTests.PostConfigurations
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Common.Constant;
    using Core.Configurations;
    using Core.Services;
    using IdentityModel;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Http;
    using Moq;
    using Xunit;

    public class CustomJwtBearerPostConfigureOptionsTest
    {
        private readonly Mock<IOAuthService> _mockOAuthService;
        private readonly CustomJwtBearerPostConfigureOptions _postConfigureOptions;
        private readonly JwtBearerOptions _jwtBearerOptions;

        public CustomJwtBearerPostConfigureOptionsTest()
        {
            _mockOAuthService = new Mock<IOAuthService>();
            _postConfigureOptions = new CustomJwtBearerPostConfigureOptions(_mockOAuthService.Object);
            _jwtBearerOptions = new JwtBearerOptions
            {
                Events = new JwtBearerEvents()
            };
        }

        [Fact]
        public void PostConfigure_SetsOnTokenValidatedEvent()
        {
            // Arrange
            var originalOnTokenValidated = _jwtBearerOptions.Events.OnTokenValidated;

            // Act
            _postConfigureOptions.PostConfigure("test", _jwtBearerOptions);

            // Assert
            Assert.NotEqual(originalOnTokenValidated, _jwtBearerOptions.Events.OnTokenValidated);
            Assert.NotNull(_jwtBearerOptions.Events.OnTokenValidated);
        }

        [Fact]
        public async Task OnTokenValidated_WithValidToken_CreatesNewPrincipalWithCombinedClaims()
        {
            // Arrange
            var accessToken = "valid-token";
            var originalClaims = new List<Claim>
            {
                new Claim(CustomClaimType.IdentityAccessToken, accessToken),
                new Claim("original-claim", "original-value")
            };

            var sessionClaims = new List<Claim>
            {
                new Claim(CustomClaimType.SiteName, "TestSite"),
                new Claim("session-claim", "session-value")
            };

            var originalPrincipal = new ClaimsPrincipal(new ClaimsIdentity(originalClaims));
            var context = CreateTokenValidatedContext(originalPrincipal);

            _mockOAuthService.Setup(x => x.GetUserInfo(accessToken))
                .ReturnsAsync(sessionClaims);

            _postConfigureOptions.PostConfigure("test", _jwtBearerOptions);

            // Act
            await _jwtBearerOptions.Events.OnTokenValidated(context);

            // Assert
            Assert.NotEqual(originalPrincipal, context.Principal);
            Assert.Equal(4, context.Principal.Claims.ToList().Count); // 2 session + 2 original
            
            // Verify session claims are included
            Assert.Contains(context.Principal.Claims, c => c.Type == CustomClaimType.SiteName && c.Value == "TestSite");
            Assert.Contains(context.Principal.Claims, c => c.Type == "session-claim" && c.Value == "session-value");
            
            // Verify original claims are preserved
            Assert.Contains(context.Principal.Claims, c => c.Type == CustomClaimType.IdentityAccessToken && c.Value == accessToken);
            Assert.Contains(context.Principal.Claims, c => c.Type == "original-claim" && c.Value == "original-value");
        }

        [Fact]
        public async Task OnTokenValidated_WithNullSessionClaims_FailsContext()
        {
            // Arrange
            var accessToken = "invalid-token";
            var originalClaims = new List<Claim>
            {
                new Claim(CustomClaimType.IdentityAccessToken, accessToken)
            };

            var originalPrincipal = new ClaimsPrincipal(new ClaimsIdentity(originalClaims));
            var context = CreateTokenValidatedContext(originalPrincipal);

            _mockOAuthService.Setup(x => x.GetUserInfo(accessToken))
                .ReturnsAsync((IEnumerable<Claim>)null);

            _postConfigureOptions.PostConfigure("test", _jwtBearerOptions);

            // Act
            await _jwtBearerOptions.Events.OnTokenValidated(context);

            // Assert
            Assert.NotNull(context.Result);
            Assert.True(context.Result.Failure != null);
            Assert.Equal("Unauthorized", context.Result.Failure.Message);
        }

        [Fact]
        public async Task OnTokenValidated_WithMissingAccessTokenClaim_CallsOAuthServiceWithNull()
        {
            // Arrange
            var originalClaims = new List<Claim>
            {
                new Claim("other-claim", "other-value")
            };

            var sessionClaims = new List<Claim>
            {
                new Claim(CustomClaimType.SiteName, "TestSite")
            };

            var originalPrincipal = new ClaimsPrincipal(new ClaimsIdentity(originalClaims));
            var context = CreateTokenValidatedContext(originalPrincipal);

            _mockOAuthService.Setup(x => x.GetUserInfo(null))
                .ReturnsAsync(sessionClaims);

            _postConfigureOptions.PostConfigure("test", _jwtBearerOptions);

            // Act
            await _jwtBearerOptions.Events.OnTokenValidated(context);

            // Assert
            _mockOAuthService.Verify(x => x.GetUserInfo(null), Times.Once);
            Assert.Equal(2, context.Principal.Claims.ToList().Count); // 1 session + 1 original
        }

        [Fact]
        public async Task OnTokenValidated_CreatesClaimsIdentityWithCorrectAuthenticationTypeAndRoleType()
        {
            // Arrange
            var accessToken = "valid-token";
            var originalClaims = new List<Claim>
            {
                new Claim(CustomClaimType.IdentityAccessToken, accessToken)
            };

            var sessionClaims = new List<Claim>
            {
                new Claim(CustomClaimType.SiteName, "TestSite")
            };

            var originalPrincipal = new ClaimsPrincipal(new ClaimsIdentity(originalClaims));
            var context = CreateTokenValidatedContext(originalPrincipal);

            _mockOAuthService.Setup(x => x.GetUserInfo(accessToken))
                .ReturnsAsync(sessionClaims);

            _postConfigureOptions.PostConfigure("test", _jwtBearerOptions);

            // Act
            await _jwtBearerOptions.Events.OnTokenValidated(context);

            // Assert
            var identity = context.Principal.Identity as ClaimsIdentity;
            Assert.NotNull(identity);
            Assert.Equal(JwtBearerDefaults.AuthenticationScheme, identity.AuthenticationType);
            Assert.Equal(CustomClaimType.SiteName, identity.NameClaimType);
            Assert.Equal(JwtClaimTypes.Role, identity.RoleClaimType);
        }

        [Fact]
        public async Task OnTokenValidated_CallsOriginalOnTokenValidatedFirst()
        {
            // Arrange
            var originalOnTokenValidatedCalled = false;
            var accessToken = "valid-token";
            var originalClaims = new List<Claim>
            {
                new Claim(CustomClaimType.IdentityAccessToken, accessToken)
            };

            var sessionClaims = new List<Claim>
            {
                new Claim(CustomClaimType.SiteName, "TestSite")
            };

            var originalPrincipal = new ClaimsPrincipal(new ClaimsIdentity(originalClaims));
            var context = CreateTokenValidatedContext(originalPrincipal);

            _jwtBearerOptions.Events.OnTokenValidated = ctx =>
            {
                originalOnTokenValidatedCalled = true;
                return Task.CompletedTask;
            };

            _mockOAuthService.Setup(x => x.GetUserInfo(accessToken))
                .ReturnsAsync(sessionClaims);

            _postConfigureOptions.PostConfigure("test", _jwtBearerOptions);

            // Act
            await _jwtBearerOptions.Events.OnTokenValidated(context);

            // Assert
            Assert.True(originalOnTokenValidatedCalled);
        }

        [Theory]
        [InlineData("token1")]
        [InlineData("token2")]
        [InlineData("special-token-123")]
        public async Task OnTokenValidated_WithDifferentTokens_CallsOAuthServiceWithCorrectToken(string token)
        {
            // Arrange
            var originalClaims = new List<Claim>
            {
                new Claim(CustomClaimType.IdentityAccessToken, token)
            };

            var sessionClaims = new List<Claim>
            {
                new Claim(CustomClaimType.SiteName, "TestSite")
            };

            var originalPrincipal = new ClaimsPrincipal(new ClaimsIdentity(originalClaims));
            var context = CreateTokenValidatedContext(originalPrincipal);

            _mockOAuthService.Setup(x => x.GetUserInfo(token))
                .ReturnsAsync(sessionClaims);

            _postConfigureOptions.PostConfigure("test", _jwtBearerOptions);

            // Act
            await _jwtBearerOptions.Events.OnTokenValidated(context);

            // Assert
            _mockOAuthService.Verify(x => x.GetUserInfo(token), Times.Once);
        }

        private TokenValidatedContext CreateTokenValidatedContext(ClaimsPrincipal principal)
        {
            var httpContext = new DefaultHttpContext();
            var authenticationScheme = new AuthenticationScheme(
                JwtBearerDefaults.AuthenticationScheme,
                JwtBearerDefaults.AuthenticationScheme,
                typeof(JwtBearerHandler)
            );

            return new TokenValidatedContext(httpContext, authenticationScheme, _jwtBearerOptions)
            {
                Principal = principal
            };
        }
    }
}