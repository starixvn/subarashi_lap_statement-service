

using Fanex.Athena.ExtService.Common.Constant;
using Fanex.Athena.ExtService.Core.Models;
using IdentityModel.Client;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fanex.Athena.ExtService.Core.Services
{
    public class OAuthService : IOAuthService
    {
        private const string PrefixCacheKey = "OAuth_Access_Token_";

        private readonly IdentitySetting _identitySetting;
        private readonly TokenManagement _tokenManagement;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;

        public OAuthService(IHttpClientFactory httpClientFactory, IOptions<IdentitySetting> identityOptions, IOptions<TokenManagement> tokenOptions, IMemoryCache memoryCache) 
        {
            _httpClientFactory = httpClientFactory;
            _identitySetting = identityOptions.Value;
            _tokenManagement = tokenOptions.Value;
            _cache = memoryCache;
        }

        public async Task<IEnumerable<Claim>> GetUserInfo(string accessToken)
        {
            var key = $"{PrefixCacheKey}{accessToken}";
            if (_cache.TryGetValue(key, out IEnumerable<Claim> claims))
            {
                return claims;
            }
            var httpClient = _httpClientFactory.CreateClient();
            var userInfo = await httpClient.GetUserInfoAsync(new UserInfoRequest
            {
                Address = $"{_identitySetting.Authority}/{_identitySetting.UserInfoPath}",
                Token = accessToken
            });

            claims = userInfo?.Claims;
            if (claims is not null && claims.Any())
            {
                _cache.Set(key, claims, TimeSpan.FromMinutes(_tokenManagement.AccessExpiration));
            }
            return userInfo.Claims;
        }

        public async Task<string> RequestTokenAsync(Models.TokenRequest request)
        {
            var siteName = request.Username;
            var realUserName = request.Username;

            if (request.Username.Contains("@"))
            {
                var temp = request.Username.Split('@');
                siteName = temp[0];
                realUserName = temp[1];
            }
            var parameters = new Parameters
            {
                    { "sitename", siteName },
                    { "username", realUserName  },
                    { "isshowipvn", request.IsShowIpvn.ToString() }
            };
            var response = await RequestTokenFromIdentityServerAsync(parameters);
            var accessToken = response.AccessToken;
            return accessToken;
        }

        private Task<TokenResponse> RequestTokenFromIdentityServerAsync(Parameters parameters)
        {
            var httpClient = _httpClientFactory.CreateClient();
            return httpClient.RequestTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = $"{_identitySetting.Authority}/{_identitySetting.TokenPath}",
                ClientId = _identitySetting.ClientId,
                ClientSecret = _identitySetting.ClientSecret,
                Scope = _identitySetting.Scope,
                GrantType = GrantType.StatementGrantType,
                Parameters = parameters
            });
        }
    }
}
