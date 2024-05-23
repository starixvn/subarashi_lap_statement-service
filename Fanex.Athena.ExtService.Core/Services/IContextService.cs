using System.Collections.Generic;

namespace Fanex.Athena.ExtService.Core.Services
{
    public interface IContextService
    {
        string OAuthAccessToken { get; }

        string SiteName { get; }

        string UserName { get; }

        string SiteDBServer { get; }

        IReadOnlyDictionary<int, int[]> AgentList { get; }

        IReadOnlyDictionary<int, string> CurrencyList { get; }

        bool IsAllowedCustomerComm { get; }

        bool IsEnableBA { get; }

        bool IsEnableSingleWallet { get; }

        bool IsFakeLicensee { get; }

        IEnumerable<string> OfferedSubProductList { get; }

        bool IsInRoles(params string[] roles);

        int SpecialPlatform { get; }

        int SpecialPlatformType { get; }

        bool IsShowIP { get; }

        bool IsShowIpvn { get; }
    }
}
