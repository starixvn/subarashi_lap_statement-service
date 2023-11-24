using System.Threading.Tasks;

using Fanex.Athena.ExtService.Common.Models.DTO;

namespace Fanex.Athena.ExtService.Common.Services
{
    public interface ICommonService
    {
        Task<LicenseeInfo> GetLicenseeInfoAsync(string siteName, int custId = 0);

        Task<string> GetSiteDBServer(string licenseeName);

        Task<BalanceUp> GetBalanceUpAsync();

        Task<bool> IsEnableSingleWallet(string siteName);

        bool IsShowIp(string siteName);

        Task<int> GetSpecialPlatform(string licenseeName);

    }
}