using System;
using System.Collections.Generic;
using Fanex.Athena.ExtService.Shared.DTO;
using Fanex.Athena.ExtService.Shared.Model.DTO;
using Fanex.Data;

using Microsoft.Extensions.Caching.Memory;

namespace Fanex.Athena.ExtService.Shared.Services
{
    public class CommonService : ICommonService
    {
        private const int CacheTimeoutSeconds = 120;

        private const string BalanceUpCacheKey = "EXT_LAP_CACHE_BALANCEUP";

        private readonly IMemoryCache _cache;
        public CommonService(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }
        public LicenseeInfo GetLicenseeInfo(string siteName, int custId = 0)
        {
            const string UspName = "Lrf_LicenseeSelExtInfo";
            var result = new LicenseeInfo
            {
                SiteName = siteName
            };
            using (var db = ObjectDbFactory.CreateInstance(UspName))
            {
                db.SetParameterValue("siteName", siteName);
                db.SetParameterValue("custId", custId);
                using (var reader = db.ExecuteReader())
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

        public BalanceUp GetBalanceUp()
        {
            var balanceup = _cache.Get<BalanceUp>(BalanceUpCacheKey);

            if (balanceup != null)
            {
                return balanceup;
            }

            balanceup = GetBalanceUpDayFromDb();
            _cache.Set(BalanceUpCacheKey, balanceup, DateTimeOffset.Now.AddSeconds(CacheTimeoutSeconds));
            return balanceup;
        }

        private static BalanceUp GetBalanceUpDayFromDb()
        {
            const string UspName = "Lrf_SelBalanceUpDay";

            using (var db = ObjectDbFactory.CreateInstance(UspName))
            {
                var balanceUp = new BalanceUp();
                using (var reader = db.ExecuteReader())
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
