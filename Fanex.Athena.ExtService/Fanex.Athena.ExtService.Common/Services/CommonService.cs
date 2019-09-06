using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Fanex.Athena.ExtService.Common.Models.DTO;
using Fanex.Data;

using Microsoft.Extensions.Caching.Memory;

namespace Fanex.Athena.ExtService.Common.Services
{
    public class CommonService : ICommonService
    {
        private const int CacheTimeout120Seconds = 120;
        private const int CacheTimeout300Seconds = 300;

        private const string BalanceUpCacheKey = "EXT_LAP_CACHE_BALANCEUP";
        private const string LicenseeExtInfoCacheKey = "__LicenseeExtInfoCacheKey_";
        private readonly IMemoryCache _cache;
        public CommonService(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }
        public async Task<LicenseeInfo> GetLicenseeInfoAsync(string siteName, int custId = 0)
        {
            _cache.TryGetValue(LicenseeExtInfoCacheKey, out LicenseeInfo result);

            if (result is null)
            {
                result = await GetLicenseeInfoFromDB(siteName, custId);
                _cache.Set(LicenseeExtInfoCacheKey, result, DateTimeOffset.Now.AddSeconds(CacheTimeout300Seconds));
            }

            return result;
        }

        private static async Task<LicenseeInfo> GetLicenseeInfoFromDB(string siteName, int custId)
        {
            const string UspName = "Lrf_LicenseeSelExtInfo";

            var result = new LicenseeInfo
            {
                SiteName = siteName
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

        public async Task<BalanceUp> GetBalanceUpAsync()
        {
            var balanceup = _cache.Get<BalanceUp>(BalanceUpCacheKey);

            if (balanceup != null)
            {
                return balanceup;
            }

            balanceup = await GetBalanceUpDayFromDb();
            _cache.Set(BalanceUpCacheKey, balanceup, DateTimeOffset.Now.AddSeconds(CacheTimeout120Seconds));
            return balanceup;
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
    }
}
