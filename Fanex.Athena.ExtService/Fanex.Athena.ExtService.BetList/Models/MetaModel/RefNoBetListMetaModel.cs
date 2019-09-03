using System;
using System.Collections.Generic;
using System.Text;

namespace Fanex.Athena.ExtService.BetList.Models.MetaModel
{
    public class RefNoBetListMetaModel
    {
        public long RefNo { get; set; }
        public string BaseCurrency { get; set; }
        public string AgentListXml { get; set; }
        public string UserName { get; set; }
        public IDictionary<int, string> UserCurrencyList { get; set; }
        public bool HasPlayerCom { get; set; }
    }
}
