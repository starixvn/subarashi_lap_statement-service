using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Fanex.Athena.ExtService.BetList.Models.MetaModel
{
    [JsonObject("BetListDetailByProductMetaModel")]
    public class BetListDetailByProductMetaModel : IBaseBetListMetaModel
    {
        public long TransId { get; set; }

        public long TransType { get; set; }

        public int CustId { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public string UspKey { get; set; }

        public bool HasPlayerCom { get; set; }
        public bool HasTotal { get; set; }
        public bool HasLicWl { get; set; }
        public long RefCode { get; set; }
        [JsonProperty("CurrencyList")]
        public IDictionary<int, string> UserCurrencyList { get; set; }
        [JsonProperty("AgentListXml")]
        public string AgentListXml { get; set; }
        public string BaseCurrency { get; set; }
        public string Lang { get; set; }
    }
}
