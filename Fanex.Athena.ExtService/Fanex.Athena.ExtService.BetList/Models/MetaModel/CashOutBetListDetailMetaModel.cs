using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Fanex.Athena.ExtService.BetList.Models.MetaModel
{
    [JsonObject("CashOutBetListDetailMetaModel")]
    public class CashOutBetListDetailMetaModel : IBaseBetListMetaModel
    {
        public long TransId { get; set; }

        public long RefNo { get; set; }

        public int BetType { get; set; }

        public long BetdaqId { get; set; }

        public bool IsCashOut { get; set; }

        public bool HasPlayerComm { get; set; }

        public IDictionary<int, string> UserCurrencyList { get; set; }

        [JsonProperty("AgentListXml")]
        public string AgentListXml { get; set; }
        public string BaseCurrency { get; set; }
        public string Lang { get; set; }
        public bool HasPlayerCom { get; set; }
    }
}
