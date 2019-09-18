using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Fanex.Athena.ExtService.Common.Models.DTO;
using Fanex.Data;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace Fanex.Athena.ExtService.Common.Services
{
    public class CommonService : ICommonService
    {
        private const int CacheTimeout120Seconds = 120;
        private const int CacheTimeout300Seconds = 300;
        private const int CacheTimeOut30Seconds = 30;
        private const string BalanceUpCacheKey = "EXT_LAP_CACHE_BALANCEUP";
        private const string LicenseeExtInfoCacheKey = "__LicenseeExtInfoCacheKey_";
        private readonly IDistributedCache _cache;

        public CommonService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<LicenseeInfo> GetLicenseeInfoAsync(string siteName, int custId = 0)
        {
            LicenseeInfo result = null;
            var realCacheKey = $"{LicenseeExtInfoCacheKey}{siteName}_{custId}";
            var cachedValue = await GetValueFromCacheAsync(realCacheKey);

            if (custId > 0 && cachedValue is null)
            {
                var cacheKeyBySiteName = $"{LicenseeExtInfoCacheKey}{siteName}_0";
                cachedValue = await GetValueFromCacheAsync(cacheKeyBySiteName);
            }

            if (cachedValue != null)
            {
                result = JsonConvert.DeserializeObject<LicenseeInfo>(cachedValue);
            }

            if (result is null)
            {
                result = await GetLicenseeInfoFromDBAsync(siteName, custId);

                var options = custId > 0
                    ? WithSlidingExpiration(TimeSpan.FromSeconds(CacheTimeOut30Seconds))
                    : WithAbsoluteExpiration(DateTimeOffset.Now.AddSeconds(CacheTimeout300Seconds));
                await SetValueToCacheAsync(realCacheKey, JsonConvert.SerializeObject(result), options);
            }

            return result ?? new LicenseeInfo { SiteName = siteName, Currencies = new CurrencyInfo[] { } };
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

        public async Task<BalanceUp> GetBalanceUpAsync()
        {
            var cachedValue = await GetValueFromCacheAsync(BalanceUpCacheKey);
            if (cachedValue != null)
            {
                return JsonConvert.DeserializeObject<BalanceUp>(cachedValue);
            }

            var balanceup = await GetBalanceUpDayFromDb();
            var options = WithAbsoluteExpiration(DateTimeOffset.Now.AddSeconds(CacheTimeout120Seconds));
            await SetValueToCacheAsync(BalanceUpCacheKey, JsonConvert.SerializeObject(balanceup), options);

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

        private static DistributedCacheEntryOptions WithAbsoluteExpiration(DateTimeOffset time)
           => new DistributedCacheEntryOptions { AbsoluteExpiration = time };

        private static DistributedCacheEntryOptions WithSlidingExpiration(TimeSpan time)
            => new DistributedCacheEntryOptions { SlidingExpiration = time };

        private async Task<string> GetValueFromCacheAsync(string key)
        {
            var value = await _cache.GetAsync(key);
            return value is null ? null : Encoding.UTF8.GetString(value);
        }

        private Task SetValueToCacheAsync(string key, string value, DistributedCacheEntryOptions options)
        {
            return _cache.SetAsync(key, Encoding.UTF8.GetBytes(value), options);
        }
    }
}
