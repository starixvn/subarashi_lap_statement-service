using System;
using Fanex.Athena.ExtService.BetList.Models.MetaModel;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.ExtService.BetList.Controllers
{
    public class BetListByGroupMetaModel : IBaseMetaModel
    {
        [FromQuery(Name = "bc")]
        public string BaseCurrency { get; set; }
        public string Lang { get; set; }
        public bool IsValid => true;

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
        public string ModeView { get; set; }

    }
}