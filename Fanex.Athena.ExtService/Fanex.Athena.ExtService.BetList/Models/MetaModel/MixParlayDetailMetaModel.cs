using System;
using System.Collections.Generic;
using System.Text;

namespace Fanex.Athena.ExtService.BetList.Models.MetaModel
{
    public class MixParlayDetailMetaModel
    {
        public long TransId { get; set; }

        public int CustId { get; set; }

        public IDictionary<int, string> UserCurrencyList { get; set; }
        public string Lang { get; set; }
    }
}
