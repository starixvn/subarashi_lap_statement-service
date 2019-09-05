using System;
using Fanex.Athena.ExtService.BetList.Models.MetaModel;
using Fanex.Athena.ExtService.Common.Model.DataDomain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Fanex.Athena.ExtService.BetList.Models.MetaModel
{
    public class SummaryStatementMetaModel : IBaseMetaModel
    {
        [FromQuery(Name = "cid")]
        public int CustId { get; set; }
        [FromQuery(Name = "fd")]
        public DateTime FromDate { get; set; }
        [FromQuery(Name = "td")]
        public DateTime ToDate { get; set; }
        [FromQuery(Name = "ps")]
        public int PageSize { get; set; } = 50;
        [FromQuery(Name = "pi")]
        public int PageIndex { get; set; } = 1;
        [FromQuery(Name = "bc")]
        public string BaseCurrency { get; set; } = SystemConstants.DefaultBaseCurrency;
        public string Lang { get; set; } = SystemConstants.DefaultLangCode;

        public bool IsValid => CustId > 0 && FromDate <= ToDate && (ToDate - FromDate).TotalDays <= 31
            && PageSize >= 1 && PageSize >= 1;
    }
}