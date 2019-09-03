using System;
using System.Collections.Generic;
using System.Text;

namespace Fanex.Athena.ExtService.BetList.Models.MetaModel
{
    public class SystemParlayDetailMetaModel
    {
        public long RefNo { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }
        public string AgentListXml { get; set; }

        public string BaseCurrency { get; set; }
        public string Lang { get; set; }
    }
}
