using Fanex.Athena.ExtService.Common.Models.DataDomain;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.ExtService.BetList.Models.MetaModel
{
    public class RefNoBetListMetaModel : IBaseBetListMetaModel
    {
        [FromQuery(Name = "refId")]
        public long RefNo { get; set; }
        [FromQuery(Name = "urs")]
        public string UserName { get; set; } = string.Empty;
        public bool HasPlayerCom { get; set; }
        public string AgentListXml { get; set; }

        [FromQuery(Name = "bc")]
        public string BaseCurrency { get; set; } = SystemConstants.DefaultBaseCurrency;
        public string Lang { get; set; } = SystemConstants.DefaultLangCode;

        public bool IsDesktopViewOnly { get; set; }

        public bool IsValid => RefNo > 0;
    }
}
