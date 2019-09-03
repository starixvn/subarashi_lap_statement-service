using System;
using System.Collections.Generic;
using System.Text;

namespace Fanex.Athena.ExtService.BetList.Models.MetaModel
{
    public class BetListDetailByProductMetaModel : BaseMetaModel
    {
        public long TransId { get; set; }

        public long TransType { get; set; }

        public int CustId { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public string UspKey { get; set; }

        public IDictionary<int, string> CurrencyList { get; set; }

        public bool HasPlayerCom { get; set; }
        public bool HasTotal { get; set; }
        public bool HasLicWl { get; set; }
        public long RefCode { get; set; }
    }
}
