using System;
using System.Collections.Generic;
using Fanex.Athena.ExtService.BetList.Models.DataDomain;
using Fanex.Athena.ExtService.BetList.Models.MetaModel;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.ExtService.BetList.Controllers
{
    public class BetListByGroupMetaModel : IBaseBetListMetaModel
    {
        [FromQuery(Name = "bc")]
        public string BaseCurrency { get; set; }
        public string Lang { get; set; }
        public bool IsValid => PageIndex >
            0 && PageSize > 0 && CustId > 0 && Date > DateTime.MinValue
            && !string.IsNullOrEmpty(ProductCode) && TransDescValue.MaptoModeView.Values.Contains(TransType);

        [FromQuery(Name = "pi")]
        public int PageIndex { get; set; } = 1;

        [FromQuery(Name = "ps")]
        public int PageSize { get; set; } = 500;

        [FromQuery(Name = "cid")]
        public int CustId { get; set; }

        [FromQuery(Name = "wd")]
        public DateTime Date { get; set; }
        [FromQuery(Name = "pc")]
        public string ProductCode { get; set; }

        [FromQuery(Name = "s")]
        public string Sort { get; set; } = "Transid ASC";

        [FromQuery(Name = "mv")]
        public int TransType { get; set; }

        public IDictionary<int, string> UserCurrencyList { get; set; }
        public bool HasPlayerCom { get; set; }
        public string AgentList { get; set; }

    }
}