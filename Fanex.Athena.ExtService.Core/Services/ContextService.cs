namespace Fanex.Athena.ExtService.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Security.Claims;
    using Common.Constant;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;

    public class ContextService : IContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private IReadOnlyDictionary<int, int[]> _agentList;

        private IReadOnlyDictionary<int, string> _currencyList;

        private IEnumerable<CurrencyAgentInfo> _rawCurrencyAgentList;

        public ContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected HttpContext Context => _httpContextAccessor.HttpContext;

        protected ClaimsPrincipal User => Context.User;

        public bool IsAdministrator => GetClaimValue<bool>("Administrator");

        public string ClientIP => Context.Connection.RemoteIpAddress.ToString();

        protected internal IEnumerable<CurrencyAgentInfo> RawCurrencyAgentList
        {
            get
            {
                if (_rawCurrencyAgentList is null)
                {
                    var agentClaims = User.Claims.Where(_ => _.Type.Equals("CurrencyAgent", StringComparison.OrdinalIgnoreCase));
                    _rawCurrencyAgentList = agentClaims.Select(agent => JsonConvert.DeserializeObject<CurrencyAgentInfo>(agent.Value));
                }

                return _rawCurrencyAgentList;
            }
        }

        public string OAuthAccessToken => GetClaimValue<string>(CustomClaimType.IdentityAccessToken);

        public bool IsAllowedCustomerComm => GetClaimValue<bool>("IsAllowedCustomerComm");

        public bool IsEnableSingleWallet => GetClaimValue<bool>("IsEnableSingleWallet");

        public string SiteName => GetClaimValue("SiteName");

        public int SpecialPlatform => GetClaimValue<int>("SpecialPlatform");

        public int SpecialPlatformType => GetClaimValue<int>("SpecialPlatformType");

        public string UserName => GetClaimValue("UserID");

        public bool IsFakeLicensee => GetClaimValue<bool>("IsFakeLicensee");

        public string SiteDBServer => GetClaimValue("SiteDBServer");

        public IEnumerable<string> OfferedSubProductList
        {
            get
            {
                var offeredSubProductList = GetClaimValue("OfferedSubProducts");
                return string.IsNullOrEmpty(offeredSubProductList) ? Enumerable.Empty<string>() : offeredSubProductList.Split(',');
            }
        }

        public bool IsInRoles(params string[] roles) => Array.Exists(roles, x => User.IsInRole(x));

        public IReadOnlyDictionary<int, int[]> AgentList
        {
            get
            {
                if (_agentList is null)
                {
                    var currencyAgentList = RawCurrencyAgentList;
                    var dicClaims = currencyAgentList.ToDictionary(_ => _.CurrencyId, _ => _.AgentId);
                    _agentList = new ReadOnlyDictionary<int, int[]>(dicClaims);
                }

                return _agentList;
            }
        }

        public IReadOnlyDictionary<int, string> CurrencyList
        {
            get
            {
                if (_currencyList is null)
                {
                    var currencyAgentList = RawCurrencyAgentList;
                    var dicClaims = currencyAgentList.ToDictionary(_ => _.CurrencyId, _ => _.Currency);
                    _currencyList = new ReadOnlyDictionary<int, string>(dicClaims);
                }

                return _currencyList;
            }
        }

        public bool IsShowIP => GetClaimValue<bool>("IsShowIP");

        public bool IsShowIpvn => GetClaimValue<bool>("IShowIPVN");

        private T GetClaimValue<T>(string claimType)
        {
            var value = GetClaimValue(claimType);
            return value is null ? default : (T)Convert.ChangeType(value, typeof(T));
        }

        private string GetClaimValue(string claimType)
            => User.FindFirst(claimType)?.Value;

        protected internal class CurrencyAgentInfo
        {
            public int[] AgentId { get; set; }

            public int CurrencyId { get; set; }

            public string Currency { get; set; }
        }
    }
}