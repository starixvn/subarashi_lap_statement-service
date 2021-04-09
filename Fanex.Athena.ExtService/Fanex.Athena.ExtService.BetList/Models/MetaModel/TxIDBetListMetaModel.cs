using System;
using Fanex.Athena.ExtService.Common.Models.DataDomain;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.ExtService.BetList.Models.MetaModel
{
    public class TxIDBetListMetaModel : IBaseBetListMetaModel
    {
        [FromQuery(Name = "txId")]
        public string TxID { get; set; }

        [FromQuery(Name = "urs")]
        public string UserName { get; set; } = string.Empty;

        [FromQuery(Name = "cid")]
        public int CustId { get; set; }

        public bool HasPlayerCom { get; set; }

        public bool IsShowIpvn { get; set; } = true;

        public bool IsShowIp { get; set; }

        public string AgentListXml { get; set; }

        [FromQuery(Name = "bc")]
        public string BaseCurrency { get; set; } = SystemConstants.DefaultBaseCurrency;

        public string Lang { get; set; } = SystemConstants.DefaultLangCode;

        public bool IsDesktopViewOnly { get; set; }

        public bool IsValid => !String.IsNullOrEmpty(TxID);
    }
}
