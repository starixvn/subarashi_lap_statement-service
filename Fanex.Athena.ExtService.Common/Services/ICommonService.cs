namespace Fanex.Athena.ExtService.Common.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models.DTO;

    public interface ICommonService
    {
        Task<LicenseeInfo> GetLicenseeInfoAsync(string siteName, int custId = 0);

        Task<string> GetSiteDBServer(string licenseeName);

        Task<BalanceUp> GetBalanceUpAsync();

        Task<bool> IsEnableSingleWallet(string siteName);

        bool IsShowIp(string siteName);

        int GetSpecialPlatform(string licenseeName);

        Task<List<string>> GetWhitelistIPFromDB(string siteName);

        Task<Dictionary<int, string>> GetAllCurrencyInfo();

        Task<Dictionary<string, string>> GetAllProductResource(string languageCode);
    }
}