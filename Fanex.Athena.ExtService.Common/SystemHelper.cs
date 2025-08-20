namespace Fanex.Athena.ExtService.Common
{
    using System.Linq;
    using System.Text;
    using Models.DataDomain;
    using Models.DTO;

    public static class SystemHelper
    {
        public static string GenerateAgentListXml(LicenseeInfo licenseeInfo, bool isExcludeTestData = false)
        {
            var result = new StringBuilder("<ROOT>");
            var currencies = isExcludeTestData ?
                licenseeInfo.Currencies.Where(x => x.CurrencyId != CurrencyType.Uus).ToArray() : licenseeInfo.Currencies;

            foreach (var item in currencies)
            {
                result.AppendFormat("<Agent AgentId=\"{0}\" Currency=\"{1}\"/>", item.AgentId, item.CurrencyId);
            }

            result.Append("</ROOT>");
            return result.ToString();
        }
    }
}