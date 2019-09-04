using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Athena.ExtService.Common.DTO;
using Fanex.Athena.ExtService.Common.Model.DataDomain;

namespace Fanex.Athena.ExtService.Common
{
    public static class SystemHelper
    {
        public static string GenerateAgentListXml(LicenseeInfo licenseeInfo, bool isExcludeTestData = false)
        {
            var result = new StringBuilder("<ROOT>");
            var currencies = isExcludeTestData ?
                licenseeInfo.Currencies.Where(x => x.CurrencyId != CurrencyType.Uus).ToArray() : licenseeInfo.Currencies;

            foreach(var item in currencies)
            {
                result.AppendFormat("<Agent AgentId=\"{0}\" Currency=\"{1}\"/>", item.AgentId, item.CurrencyId);
            }

            result.Append("</ROOT>");
            return result.ToString();
        }
    }
}
