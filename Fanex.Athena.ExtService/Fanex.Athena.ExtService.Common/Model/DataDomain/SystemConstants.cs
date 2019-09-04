using System.Collections.Generic;

namespace Fanex.Athena.ExtService.Common.Model.DataDomain
{
    public class SystemConstants
    {
        public static readonly IDictionary<int, string> BaseCurrencyList = new Dictionary<int, string> {

            { BaseCurrencyType.Eur,"EUR" },
            { BaseCurrencyType.Usd,"USD" },
            { BaseCurrencyType.Gbp,"GBP" },
            { BaseCurrencyType.MYR,"MYR" },
            { BaseCurrencyType.CNY,"CNY" },
            { BaseCurrencyType.THB,"THB" },
            { BaseCurrencyType.IDR,"IDR" },
            { BaseCurrencyType.INH,"INH" },

        };
    }
}
