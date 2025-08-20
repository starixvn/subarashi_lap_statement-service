namespace Fanex.Athena.ExtService.Core.Configurations
{
    using System.Linq;
    using System.Security.Claims;
    using Common.Constant;
    using IdentityModel;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.Options;
    using Services;

    public class CustomJwtBearerPostConfigureOptions : IPostConfigureOptions<JwtBearerOptions>
    {
        private readonly IOAuthService _oAuthService;

        public CustomJwtBearerPostConfigureOptions(IOAuthService oAuthService)
        {
            _oAuthService = oAuthService;
        }

        public void PostConfigure(string name, JwtBearerOptions options)
        {
            var onTokenValidated = options.Events.OnTokenValidated;

            options.Events.OnTokenValidated = async context =>
            {
                await onTokenValidated(context);
                var principal = context.Principal;
                var identityAccessToken = principal.FindFirst(CustomClaimType.IdentityAccessToken)?.Value;
                var sessionClaims = (await _oAuthService.GetUserInfo(identityAccessToken))?.ToList();
                if (sessionClaims == null)
                {
                    context.Fail("Unauthorized");
                    return;
                }

                sessionClaims.AddRange(principal.Claims);
                var newPrincipal = new ClaimsPrincipal(new ClaimsIdentity(sessionClaims, JwtBearerDefaults.AuthenticationScheme, CustomClaimType.SiteName, JwtClaimTypes.Role));
                context.Principal = newPrincipal;
            };
        }

    }
}
