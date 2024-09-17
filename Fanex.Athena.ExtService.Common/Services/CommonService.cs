namespace Fanex.Athena.ExtService.Common.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Microsoft.Extensions.Options;
    using Models.DTO;

    public class CommonService : ICommonService
    {
        private const int CacheTimeout120Seconds = 120;
        private const int CacheTimeout300Seconds = 300;
        private const int CacheTimeOut30Seconds = 30;
        private const string BalanceUpCacheKey = "EXT_LAP_CACHE_BALANCEUP";
        private const string CurrencyInfoCacheKey = "EXT_LAP_CACHE_CURRENCY";
        private const string LicenseeExtInfoCacheKey = "__LicenseeExtInfoCacheKey_";
        private const string WhitelistIPCacheKey = "WhitelistIP_{0}";

        private readonly ICustomDistributedCache _cache;

        private readonly AppSettings _settings;

        private readonly Lazy<IReadOnlyDictionary<int, string>> _siteDBConfig;

        public CommonService(ICustomDistributedCache cache, IOptions<AppSettings> settings)
        {
            _cache = cache;
            _settings = settings.Value;
            _siteDBConfig = new Lazy<IReadOnlyDictionary<int, string>>(() =>
            {
                const string SpName = "Acc_SiteDBConfigSel";
                var config = new Dictionary<int, string>();
                using var db = ObjectDbFactory.CreateInstance(SpName);
                using var reader = db.ExecuteReader();
                while (reader.Read())
                {
                    config.Add(int.Parse(reader["SiteDBGroupID"].ToString()), reader["AGIP"].ToString());
                }

                return config;
            });
        }

        public async Task<bool> IsEnableSingleWallet(string siteName)
        {
            const string KeyName = "EnableSingleWallet";
            const string ExpectedKeyValue = "true";
            var env = string.Equals(_settings.Environment, "uattw", StringComparison.OrdinalIgnoreCase) ? "UAT" : "Production";
            var result = await GetConfigDetailByKeyName(siteName, env, KeyName);
            return !string.IsNullOrEmpty(result) && string.Equals(result, ExpectedKeyValue, StringComparison.OrdinalIgnoreCase);
        }

        public async Task<LicenseeInfo> GetLicenseeInfoAsync(string siteName, int custId = 0)
        {
            var realCacheKey = $"{LicenseeExtInfoCacheKey}{siteName}_{custId}";
            var result = await _cache.GetAsync<LicenseeInfo>(realCacheKey);
            if (result is null)
            {
                result = await GetLicenseeInfoFromDBAsync(siteName, custId);
                if (result.Currencies.Any())
                {
                    var options = custId > 0
                        ? CustomDistributedCache.WithSlidingExpiration(TimeSpan.FromSeconds(CacheTimeOut30Seconds))
                        : CustomDistributedCache.WithAbsoluteExpiration(DateTimeOffset.Now.AddSeconds(CacheTimeout300Seconds));
                    await _cache.SetAsync(realCacheKey, result, options);
                }
            }

            return result;
        }

        public async Task<string> GetSiteDBServer(string licenseeName)
        {
            const string UspName = "Lrf_LicenseeSelBySiteName";
            using var db = ObjectDbFactory.CreateInstance(UspName);
            using var reader = await db.ExecuteReaderAsync(new { siteName = licenseeName });
            var siteDBGroupID = reader.Read() ? Convert.ToInt32(reader["SiteDBGroupID"]) : 0;
            return siteDBGroupID != 0 && _siteDBConfig.Value.TryGetValue(siteDBGroupID, out var dbServer)
                ? dbServer
                : string.Empty;
        }

        public bool IsShowIp(string siteName)
        {
            return !string.IsNullOrEmpty(siteName);
        }

        public int GetSpecialPlatform(string licenseeName)
        {
            const string UspName = "Lrf_GetSpecialPlatform";
            using var db = ObjectDbFactory.CreateInstance(UspName);
            using var reader = db.ExecuteReader(new { siteName = licenseeName });

            var specialPlatform = reader.Read() ? Convert.ToInt32(reader["SpecialPlatform"]) : 0;

            return specialPlatform;
        }

        public async Task<BalanceUp> GetBalanceUpAsync()
        {
            var result = await _cache.GetAsync<BalanceUp>(BalanceUpCacheKey);
            if (result != null)
            {
                return result;
            }

            var balanceup = await GetBalanceUpDayFromDb();
            var options = CustomDistributedCache.WithAbsoluteExpiration(DateTimeOffset.Now.AddSeconds(CacheTimeout120Seconds));
            await _cache.SetAsync(BalanceUpCacheKey, balanceup, options);

            return balanceup;
        }

        public async Task<List<string>> GetWhitelistIPFromDB(string siteName)
        {
            var cacheKey = string.Format(WhitelistIPCacheKey, siteName);
            var result = await _cache.GetAsync<List<string>>(cacheKey);
            if (result != null)
            {
                return result;
            }

            const string UspName = "Lrf_GetWhitelistIPBySiteName";
            using var db = ObjectDbFactory.CreateInstance(UspName);
            db.SetParameterValue("SiteName", siteName);
            var whitelistIP = await db.QueryAsync<string>();

            var options = CustomDistributedCache.WithAbsoluteExpiration(DateTimeOffset.Now.AddSeconds(CacheTimeout300Seconds));
            await _cache.SetAsync(cacheKey, whitelistIP, options);

            return whitelistIP.ToList();
        }

        public async Task<Dictionary<int, string>> GetAllCurrencyInfo()
        {
            var result = await _cache.GetAsync<Dictionary<int, string>>(CurrencyInfoCacheKey);
            if (result != null)
            {
                return result;
            }

            const string UspName = "Lrf_CurrencySel";
            using var db = ObjectDbFactory.CreateInstance(UspName);

            using (var reader = await db.ExecuteReaderAsync())
            {
                var currencies = new Dictionary<int, string>();
                while (reader.Read())
                {
                    currencies.Add(
                        Convert.ToInt32(reader["Id"]),
                        Convert.ToString(reader["Name"])
                    );
                }

                if (currencies.Count > 0)
                {
                    var options = CustomDistributedCache.WithAbsoluteExpiration(DateTimeOffset.Now.AddSeconds(CacheTimeout300Seconds));
                    await _cache.SetAsync(CurrencyInfoCacheKey, currencies, options);
                }

                return currencies;
            }
        }

        private static async Task<LicenseeInfo> GetLicenseeInfoFromDBAsync(string siteName, int custId)
        {
            const string UspName = "Lrf_LicenseeSelExtInfo";

            var result = new LicenseeInfo
            {
                SiteName = siteName,
                Currencies = new CurrencyInfo[] { }
            };

            using (var db = ObjectDbFactory.CreateInstance(UspName))
            {
                db.SetParameterValue(nameof(siteName), siteName);
                db.SetParameterValue(nameof(custId), custId);
                using (var reader = await db.ExecuteReaderAsync())
                {
                    var currencies = new List<CurrencyInfo>();
                    while (reader.Read())
                    {
                        currencies.Add(new CurrencyInfo
                        {
                            CurrencyId = Convert.ToInt32(reader["CurrencyId"]),
                            AgentId = Convert.ToInt32(reader["AgentId"]),
                            CurrencyName = Convert.ToString(reader["CurrencyName"])
                        });
                    }

                    result.Currencies = currencies.ToArray();
                    if (reader.NextResult() && reader.Read())
                    {
                        result.IsAllowedCustomerComm = Convert.ToBoolean(reader["IsAllowedCustomerComm"]);
                    }
                }
            }

            return result;
        }

        private static async Task<BalanceUp> GetBalanceUpDayFromDb()
        {
            const string UspName = "Lrf_SelBalanceUpDay";

            using (var db = ObjectDbFactory.CreateInstance(UspName))
            {
                var balanceUp = new BalanceUp();
                using (var reader = await db.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        balanceUp.BalanceUpDay = Convert.ToDateTime(reader["balanceupday"]);
                        balanceUp.MoveBetsDay = Convert.ToDateTime(reader["movebetsday"]);
                    }

                    return balanceUp;
                }
            }
        }

        private static async Task<string> GetConfigDetailByKeyName(string siteName, string environment, string keyName)
        {
            const string UspName = "Lrf_GetConfigDetailByKeyName";
            using (var db = ObjectDbFactory.CreateInstance(UspName))
            {
                db.SetParameterValue("SiteName", siteName);
                db.SetParameterValue("Environment", environment);
                db.SetParameterValue("KeyName", keyName);
                db.SetParameterValue("KeyValue", null);

                await db.ExecuteNonQueryAsync();

                var keyValue = Convert.ToString(db.GetParameterValue("KeyValue"));

                return keyValue;
            }
        }
    }
}