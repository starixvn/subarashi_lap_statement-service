namespace Fanex.Athena.ExtService.Core.Services
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Common.Constant;
    using Helper;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using Models;

    public class TokenAuthenticationService : IAuthenticateService
    {
        private readonly IOAuthService _oAuthService;
        private readonly TokenManagement _tokenManagement;
        private readonly IUserManagementService _userManagementService;

        public TokenAuthenticationService(
            IUserManagementService service,
            IOAuthService oAuthService,
            IOptions<TokenManagement> tokenManagementAccessor)
        {
            _userManagementService = service;
            _tokenManagement = tokenManagementAccessor.Value;
            _oAuthService = oAuthService;
        }

        public async Task<Tuple<bool, Token>> IsAuthenticated(TokenRequest request)
        {
            var token = new Token();
            var realUserName = request.Username;

            if (request.Username.Contains("@"))
            {
                var temp = request.Username.Split('@');
                realUserName = temp[1];
            }

            if (!_userManagementService.IsValidUser(realUserName, request.Password))
            {
                return Tuple.Create(false, token);
            }

            var accessToken = await _oAuthService.RequestTokenAsync(request);
            if (accessToken == null)
            {
                return Tuple.Create(false, token);
            }

            var claim = new[]
            {
                new Claim(CustomClaimType.IdentityAccessToken, accessToken)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var encryptingCredentials = new EncryptingCredentials(key, JwtConstants.DirectKeyUseAlg,
                SecurityAlgorithms.Aes256CbcHmacSha512);
            var handler = new JwtSecurityTokenHandler();

            var jwtToken = handler.CreateJwtSecurityToken(_tokenManagement.Issuer,
                _tokenManagement.Audience,
                new ClaimsIdentity(claim),
                DateTimeHelper.Now,
                DateTimeHelper.Now.AddMinutes(_tokenManagement.AccessExpiration),
                null,
                credentials,
                encryptingCredentials
            );

            token.AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return Tuple.Create(true, token);
        }
    }
}